using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MySqlConnector;
using QuanNhauSanVuon.Dtos;
using QuanNhauSanVuon.Extensions;
using QuanNhauSanVuon.Localization;
using QuanNhauSanVuon.Services.Entities;
using QuanNhauSanVuon.Services.Enums;
using QuanNhauSanVuon.Services.Exceptions;
using QuanNhauSanVuon.Services.Interfaces;

namespace QuanNhauSanVuon.Services;

/// <inheritdoc/>
public class OrderService : IOrderService
{
    private readonly DatabaseContext _context;
    private readonly MySqlExceptionHandler _exceptionHandler;
    private readonly IAuthorizationService _authorizationService;

    public OrderService(
        DatabaseContext context,
        MySqlExceptionHandler exceptionHandler,
        IAuthorizationService authorizationService)
    {
        _context = context;
        _exceptionHandler = exceptionHandler;
        _authorizationService = authorizationService;
    }

    /// <inheritdoc/>
    public async Task<OrderListDto> GetListAsync(OrderListFiltersDto filtersDto)
    {
        int resultCount = await _context.Orders.CountAsync();
        if (resultCount == 0)
        {
            return new OrderListDto();
        }

        // Compute field and direction to sort.
        IQueryable<Order> query = _context.Orders.Include(o => o.CreatedUser);
        switch (filtersDto.SortedByField)
        {
            case nameof(SortedByFieldOption.CreatedDateTime):
                query = filtersDto.SortedByAscending
                    ? query.OrderBy(o => o.CreatedDateTime)
                    : query.OrderByDescending(o => o.CreatedDateTime);
                break;
            case nameof(SortedByFieldOption.PaidDateTime):
                query = filtersDto.SortedByAscending
                    ? query.OrderBy(o => o.PaidDateTime).ThenBy(o => o.CreatedDateTime)
                    : query.OrderByDescending(o => o.PaidDateTime)
                        .ThenByDescending(o => o.CreatedDateTime);
                break;
            default:
                throw new NotImplementedException();
        }

        // Compute seating id to filter.
        if (filtersDto.SeatingId.HasValue)
        {
            query = query.Where(o => o.SeatingId == filtersDto.SeatingId);
        }

        // Compute CreatedMonthYear to filter.
        if (filtersDto.CreatedMonthYear != null)
        {
            query = query.Where(o =>
                o.CreatedDateTime.Year == filtersDto.CreatedMonthYear.Year &&
                o.CreatedDateTime.Month == filtersDto.CreatedMonthYear.Month);
        }

        // Compute PaidMonthYear to filter.
        if (filtersDto.PaidMonthYear != null)
        {
            query = query.Where(o =>
                o.PaidDateTime.HasValue &&
                o.PaidDateTime.Value.Year == filtersDto.CreatedMonthYear.Year &&
                o.PaidDateTime.Value.Month == filtersDto.CreatedMonthYear.Month);
        }

        int pageCount = (int)Math.Ceiling((double)resultCount / filtersDto.ResultsPerPage);
        List<Order> orders = await query
            .Skip(filtersDto.ResultsPerPage * (filtersDto.Page - 1))
            .Take(filtersDto.ResultsPerPage)
            .AsSplitQuery()
            .ToListAsync();

        return new OrderListDto(pageCount, orders);
    }

    /// <inheritdoc/>
    public async Task<OrderDetailDto> GetDetailAsync(int id)
    {
        return await _context.Orders
            .Include(o => o.Items).ThenInclude(oi => oi.MenuItem)
            .Include(o => o.CreatedUser)
            .Include(o => o.LastUpdatedUser)
            .Where(o => o.Id == id)
            .Select(o => new OrderDetailDto(o))
            .SingleOrDefaultAsync()
            ?? throw new NotFoundException();
    }

    /// <inheritdoc/>
    public async Task<int> CreateAsync(OrderUpsertDto orderDto)
    {
        // Use transaction for atomic operation.
        await using IDbContextTransaction transaction = await _context.Database
            .BeginTransactionAsync();

        // Initialize the order entity.
        Order order = new Order
        {
            SeatingId = orderDto.SeatingId,
            Items = new List<OrderItem>(),
            CreatedUserId = _authorizationService.GetUserId()
        };

        _context.Orders.Add(order);

        // Prepare the existing menu item ids based on the provided ids in the DTO for
        // validation.
        IEnumerable<int> requestedMenuItemIds = orderDto.Items.Select(i => i.MenuItemId);
        List<MenuItem> existingMenuItems = await _context.MenuItems
            .Where(mi => requestedMenuItemIds.Contains(mi.Id))
            .ToListAsync();
        List<int> existingMenuItemIds = existingMenuItems
            .Select(mi => mi.Id)
            .ToList();

        // Initialize the order item entities.
        for (int index = 0; index < orderDto.Items.Count; index++)
        {
            OrderItemUpsertDto orderItemDto = orderDto.Items[index];

            // Ignore if not having been changed.
            if (!orderItemDto.HasBeenChanged)
            {
                continue;
            }

            OrderItem orderItem = InitializeNewOrderItem(
                orderItemDto,
                index,
                existingMenuItems
            );

            order.Items.Add(orderItem);
            _context.OrderItems.Add(orderItem);
        }

        // Assign the other properties.
        order.CachedNetAmount = order.Items.Sum(oi => oi.NetPricePerUnit * oi.Quantity);
        order.CachedVatAmount = order.Items.Sum(oi => oi.VatAmountPerUnit * oi.Quantity);
        order.CachedGrossAmount = order.CachedNetAmount + order.CachedVatAmount;

        // Perform the operation.
        try
        {
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            return order.Id;
        }
        catch (DbUpdateException exception)
        {
            if (exception is DbUpdateConcurrencyException)
            {
                throw new ConcurrencyException();
            }

            OperationException operationException = ConvertDbException(exception);
            if (operationException == null)
            {
                throw;
            }

            throw operationException;
        }
    }

    /// <inheritdoc/>
    public async Task UpdateAsync(int id, OrderUpsertDto orderDto)
    {
        // Use transaction for atomic operation.
        await using IDbContextTransaction transaction = await _context.Database
            .BeginTransactionAsync();

        // Prepare a list of existing menu item ids to validate the possibly invalid foreign
        // keys in the request.
        List<int> requestedMenuItemIds = orderDto.Items
            .Where(oi => oi.Id.HasValue && oi.HasBeenChanged && !oi.HasBeenDeleted)
            .Select(oi => oi.MenuItemId)
            .ToList();

        List<MenuItem> existingMenuItems = await _context.MenuItems
            .Where(mi => requestedMenuItemIds.Contains(mi.Id))
            .ToListAsync();

        IEnumerable<int> existingMenuItemIds = existingMenuItems.Select(mi => mi.Id);

        // Fetch the entity from the database.
        Order order = await _context.Orders
            .Include(o => o.Items)
            .SingleOrDefaultAsync(o => o.Id == id)
            ?? throw new NotFoundException();

        // Create new order entities or update the existing ones.
        for (int index = 0; index < order.Items.Count; index++)
        {
            OrderItemUpsertDto orderItemDto = orderDto.Items[index];
            OrderItem orderItem;

            // Ignore if not changed.
            if (!orderItemDto.HasBeenChanged)
            {
                continue;
            }

            // Create a new order item entity if not having an id.
            if (!orderItemDto.Id.HasValue)
            {
                orderItem = InitializeNewOrderItem(orderItemDto, index, existingMenuItems);
                order.Items.Add(orderItem);
                _context.OrderItems.Add(orderItem);
            }
            else
            {
                // Get the order item entity and ensure it exists.
                orderItem = order.Items
                    .SingleOrDefault(oi => oi.Id == orderItemDto.Id)
                    ?? throw OperationException.NotFound(
                        new object[]
                        {
                            nameof(orderDto.Items),
                            index,
                            nameof(orderItemDto.Id)
                        },
                        DisplayNames.OrderItem
                    );

                // Delete if having been indicated.
                if (orderItemDto.HasBeenDeleted)
                {
                    order.Items.Remove(orderItem);
                    _context.OrderItems.Remove(orderItem);
                    continue;
                }

                // Ensure the foreign key referencing to an existing menu item.
                if (orderItem.MenuItemId != orderItemDto.MenuItemId &&
                    !existingMenuItemIds.Contains(orderItemDto.MenuItemId))
                {
                    throw OperationException.NotFound(
                        new object[]
                        {
                            nameof(orderDto.Items),
                            index,
                            nameof(orderItemDto.MenuItemId)
                        },
                        DisplayNames.MenuItem
                    );
                }

                // Modify the other properties.
                orderItem.Quantity = orderItemDto.Quantity;
                orderItem.NetPricePerUnit = orderItemDto.NetPricePerUnit
                    ?? orderItem.NetPricePerUnit;
                orderItem.VatAmountPerUnit = orderItemDto.VatAmountPerUnit
                    ?? orderItem.VatAmountPerUnit;
                orderItem.MenuItemId = orderItemDto.MenuItemId;
            }
        }

        // Update the order entity.
        order.LastUpdatedDateTime = DateTime.UtcNow.ToApplicationTime();
        order.LastUpdatedUserId = _authorizationService.GetUserId();

        // Perform the operation.
        try
        {
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (DbUpdateException exception)
        {
            if (exception is DbUpdateConcurrencyException)
            {
                throw new ConcurrencyException();
            }

            OperationException operationException = ConvertDbException(exception);
            if (operationException == null)
            {
                throw;
            }

            throw operationException;
        }
    }

    /// <inheritdoc/>
    public async Task DeleteAsync(int id)
    {
        int deletedRecordCount = await _context.Orders
            .Where(o => o.Id == id)
            .ExecuteDeleteAsync();

        if (deletedRecordCount == 0)
        {
            throw new NotFoundException();
        }
    }

    /// <summary>
    /// Convert a <see cref="DbUpdateException"/> thrown by the database during the creating or
    /// updating operation into a <see cref="OperationException"/> with the details describing
    /// the cause of the error.
    /// </summary>
    /// <param name="exception">
    /// The exception thrown by the database.
    /// </param>
    /// <returns>
    /// A <see cref="OperationException"/> containing the details describing the error caused
    /// the <see cref="DbUpdateException"/>.
    /// </returns>
    private OperationException ConvertDbException(DbUpdateException exception)
    {
        if (exception.InnerException is not MySqlException mySqlException)
        {
            return null;
        }

        MySqlExceptionHandledResult handledResult = _exceptionHandler.Handle(mySqlException);
        if (!handledResult.IsForeignKeyNotFound)
        {
            return null;
        }

        string seatingIdColumnName = _context
            .GetColumnName<Order>(nameof(Order.SeatingId));
        if (handledResult.ViolatedFieldName == seatingIdColumnName)
        {
            return OperationException.NotFound(
                new object[] { nameof(OrderUpsertDto.SeatingId) },
                DisplayNames.Seating
            );
        }

        string menuItemIdColumnName = _context
            .GetColumnName<OrderItem>(nameof(OrderItem.MenuItemId));
        if (handledResult.ViolatedFieldName == menuItemIdColumnName)
        {
            return OperationException.NotFound(
                new object[] { nameof(OrderUpsertDto.Items) },
                DisplayNames.MenuItem
            );
        }

        return null;
    }

    /// <summary>
    /// Initializes a new <see cref="OrderItem"/> entity based on the data specified in the
    /// DTO.
    /// </summary>
    /// <remarks>
    /// This also checks if the <see cref="MenuItem"> specified by its id exists in the
    /// database.
    /// </remarks>
    /// <param name="orderItemDto">
    /// A DTO containing the data to initialize the new entity.
    /// </param>
    /// <param name="index">
    /// The index of the <paramref name="orderItemDto"/> in the list of DTOs provided in the
    /// request body for the creating or updating operation.
    /// </param>
    /// <param name="existingMenuItems">
    /// A <see cref="ICollection{T}"/> of the existing <see cref="MenuItem"/> entities in the
    /// database which have the ids specified in the list of <see cref="OrderItemUpsertDto"/>
    /// provided in the request body, used to check if there is any <see cref="MenuItem"/>
    /// specified but not found.
    /// </param>
    /// <returns>
    /// A new <see cref="OrderItem"/> entity if all of the provided data is valid.
    /// </returns>
    /// <exception cref="OperationException">
    /// Throws when the <see cref="MenuItem"> specified by its id in
    /// <paramref name="orderItemDto"/> doesn't exist.
    /// </exception>
    private static OrderItem InitializeNewOrderItem(
        OrderItemUpsertDto orderItemDto,
        int index,
        ICollection<MenuItem> existingMenuItems)
    {
        MenuItem menuItem = existingMenuItems
            .SingleOrDefault(mi => mi.Id == orderItemDto.MenuItemId)
            ?? throw OperationException.NotFound(
                new object[]
                {
                    nameof(OrderUpsertDto.Items),
                    index,
                    nameof(orderItemDto.MenuItemId)
                },
                DisplayNames.MenuItem
            );

        long netPricePerUnit = orderItemDto.NetPricePerUnit ?? menuItem.DefaultNetPrice;
        long vatAmountPerUnit = orderItemDto.VatAmountPerUnit
            ?? netPricePerUnit * (menuItem.DefaultVatPercentage / 100);

        return new OrderItem
        {
            Quantity = orderItemDto.Quantity,
            NetPricePerUnit = netPricePerUnit,
            VatAmountPerUnit = vatAmountPerUnit,
            MenuItem = menuItem
        };
    }
}