using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using QuanNhauSanVuon.Dtos;
using QuanNhauSanVuon.Extensions;
using QuanNhauSanVuon.Localization;
using QuanNhauSanVuon.Services.Entities;
using QuanNhauSanVuon.Services.Enums;
using QuanNhauSanVuon.Services.Exceptions;
using QuanNhauSanVuon.Services.Interfaces;

namespace QuanNhauSanVuon.Services;

/// <inheritdoc />
public class MenuItemService : IMenuItemService
{
    private readonly DatabaseContext _context;
    private readonly MySqlExceptionHandler _exceptionHandler;

    public MenuItemService(
            DatabaseContext context,
            MySqlExceptionHandler exceptionHandler)
    {
        _context = context;
        _exceptionHandler = exceptionHandler;
    }

    /// <inheritdoc />
    public async Task<MenuItemListDto> GetListAsync(MenuItemListFiltersDto filtersDto)
    {
        int resultCount = await _context.MenuItems.CountAsync();
        if (resultCount == 0)
        {
            return new MenuItemListDto();
        }

        // Compute field and direction to sort.
        IQueryable<MenuItem> query = _context.MenuItems;
        switch (filtersDto.SortedByField)
        {
            case nameof(SortedByFieldOption.Name):
                query = filtersDto.SortedByAscending
                    ? query.OrderBy(mi => mi.Name).ThenBy(mi => mi.CreatedDateTime)
                    : query.OrderByDescending(mi => mi.Name).ThenBy(mi => mi.CreatedDateTime);
                break;
            case nameof (SortedByFieldOption.DefaultNetPrice):
                query = filtersDto.SortedByAscending
                    ? query.OrderBy(mi => mi.DefaultNetPrice).ThenBy(mi => mi.Name)
                    : query.OrderByDescending(mi => mi.DefaultNetPrice).ThenBy(mi => mi.Name);
                break;
            case nameof (SortedByFieldOption.DefaultVatPercentage):
                query = filtersDto.SortedByAscending
                    ? query.OrderBy(mi => mi.DefaultVatPercentage)
                        .ThenBy(mi => mi.CreatedDateTime)
                    : query.OrderByDescending(mi => mi.DefaultVatPercentage)
                        .ThenBy(mi => mi.CreatedDateTime);
                break;
            case nameof (SortedByFieldOption.CreatedDateTime):
                query = filtersDto.SortedByAscending
                    ? query.OrderBy(mi => mi.CreatedDateTime).ThenBy(mi => mi.Name)
                    : query.OrderByDescending(mi => mi.CreatedDateTime).ThenBy(mi => mi.Name);
                break;
            default:
                throw new NotImplementedException();
        }

        // Compute category to filter.
        if (filtersDto.CategoryId.HasValue)
        {
            query = query.Where(mi => mi.CategoryId == filtersDto.CategoryId.Value);
        }

        int pageCount = (int)Math.Ceiling((double)resultCount / filtersDto.ResultsPerPage);
        List<MenuItem> menuItems = await query
            .Skip(filtersDto.ResultsPerPage * (filtersDto.Page - 1))
            .Take(filtersDto.ResultsPerPage)
            .AsSplitQuery()
            .ToListAsync();

        return new MenuItemListDto(pageCount, menuItems);
    }

    /// <inheritdoc />
    public async Task<MenuItemDetailDto> GetDetailAsync(int id)
    {
        return await _context.MenuItems
            .Where(mi => mi.Id == id)
            .Select(mi => new MenuItemDetailDto(mi))
            .SingleOrDefaultAsync()
            ?? throw new NotFoundException();
    }

    /// <inheritdoc />
    public async Task<int> CreateAsync(MenuItemUpsertDto upsertDto)
    {
        try
        {
            MenuItem menuItem = new MenuItem
            {
                Name = upsertDto.Name,
                DefaultNetPrice = upsertDto.DefaultNetPrice,
                DefaultVatPercentage = upsertDto.DefaultVatPercentage,
                Unit = upsertDto.Unit,
                ThumbnailUrl = upsertDto.ThumbnailUrl,
                CategoryId = upsertDto.CategoryId
            };

            _context.MenuItems.Add(menuItem);
            await _context.SaveChangesAsync();
            return menuItem.Id;
        }
        catch (DbUpdateException exception)
        {
            if (exception.InnerException is not MySqlException mySqlException)
            {
                throw;
            }

            bool isConvertible = TryConvertingDbException(
                mySqlException,
                out OperationException convertedException);
            if (!isConvertible)
            {
                throw;
            }

            throw convertedException;
        }
    }

    /// <inheritdoc />
    public async Task UpdateAsync(int id, MenuItemUpsertDto upsertDto)
    {
        try
        {
            int updatedRecordCount = await _context.MenuItems
                .Where(mi => mi.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(mi => mi.Name, upsertDto.Name)
                    .SetProperty(mi => mi.DefaultNetPrice, upsertDto.DefaultNetPrice)
                    .SetProperty(mi => mi.DefaultVatPercentage, upsertDto.DefaultVatPercentage)
                    .SetProperty(mi => mi.Unit, upsertDto.Unit)
                    .SetProperty(mi => mi.ThumbnailUrl, upsertDto.ThumbnailUrl)
                    .SetProperty(
                        mi => mi.LastUpdatedDateTime,
                        DateTime.UtcNow.ToApplicationTime()));
            if (updatedRecordCount == 0)
            {
                throw new NotFoundException();
            }
        }
        catch (MySqlException exception)
        {
            bool isConvertible = TryConvertingDbException(
                exception,
                out OperationException convertedException);
            if (!isConvertible)
            {
                throw;
            }

            throw convertedException;
        }
    }

    /// <inheritdoc />
    public async Task DeleteAsync(int id)
    {
        int deletedRecordCount = 0;
        try
        {
            deletedRecordCount = await _context.MenuItems
                .Where(mi => mi.Id == id)
                .ExecuteDeleteAsync();
        }
        catch (MySqlException exception)
        {
            MySqlExceptionHandledResult handledResult = _exceptionHandler.Handle(exception);
            if (!handledResult.IsDeleteOrUpdateRestricted)
            {
                throw;
            }

            int updatedRecordCount = await _context.MenuItems
                .Where(mi => mi.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(mi => mi.IsDeleted, true));

            if (updatedRecordCount == 0)
            {
                throw new NotFoundException();
            }
        }

        if (deletedRecordCount == 0)
        {
            throw new NotFoundException();
        }
    }

    /// <summary>
    /// Tries converting the exception thrown by the database during the creating or updating
    /// operation into the exception that describes the error by convention.
    /// </summary>
    /// <param name="exception">
    /// The <see cref="MySqlException"/>  instance thrown by the database during the operation.
    /// </param>
    /// <param name="convertedException">
    /// The converted exception if the conversion is successful. Otherwise, <c>null</c>.
    /// </param>
    /// <returns>
    /// A <see cref="bool"/> value indicates whether the conversion is successful.
    /// </returns>
    private bool TryConvertingDbException(
        MySqlException exception,
        out OperationException convertedException)
    {
        convertedException = null;
        MySqlExceptionHandledResult handledResult = _exceptionHandler.Handle(exception);

        if (handledResult.IsUniqueConstraintViolated)
        {
            convertedException = OperationException.Duplicated(nameof(MenuItemUpsertDto.Name));
            return true;
        }

        if (handledResult.IsForeignKeyNotFound)
        {
            convertedException = OperationException.NotFound(
                new object[] { nameof(MenuItemUpsertDto.CategoryId) },
                DisplayNames.MenuCategory);
            return true;
        }

        return false;
    }
}