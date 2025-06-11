using QuanNhauSanVuon.Dtos;
using QuanNhauSanVuon.Services.Entities;
using QuanNhauSanVuon.Services.Exceptions;

namespace QuanNhauSanVuon.Services.Interfaces;

/// <summary>
/// A service to handle order-related operations.
/// </summary>
public interface IOrderService
{
    /// <summary>
    /// Gets a list of the orders with basic information, specified by the paginating and
    /// filtering conditions.
    /// </summary>
    /// <param name="filtersDto">
    /// A DTO containing the conditions for the results.
    /// </param>
    /// <returns>
    /// A <see cref="Task{T}"/> representing the asynchronous operation, which result is the
    /// list DTO containing the orders and additional information for pagination.
    /// </returns>
    /// <exception cref="NotImplementedException">
    /// Throws when the value of the specified <c>SortedByField</c> sorting condition has not
    /// been implemented (mostly due to validation and implementation mismatch in the future
    /// when adding new fields to sort).
    /// </exception>
    Task<OrderListDto> GetListAsync(OrderListFiltersDto filtersDto);

    /// <summary>
    /// Gets an existing order with its detail information, specified by its id.
    /// </summary>
    /// <param name="id">
    /// The id of the order to retrieve.
    /// </param>
    /// <returns>
    /// A <see cref="Task{T}"/> representing the asynchronous operation, which result is the
    /// DTO containing the information of the order.
    /// </returns>
    /// <exception cref="NotFoundException">
    /// Throws when the menu item doesn't exist.
    /// </exception>
    Task<OrderDetailDto> GetDetailAsync(int id);

    /// <summary>
    /// Creates a new order.
    /// </summary>
    /// <param name="orderDto">
    /// A DTO containing the data for the creating operation.
    /// </param>
    /// <returns>
    /// A <see cref="Task{T}"/> representing the asynchronous operation, which result is the id
    /// of the created order.
    /// </returns>
    /// <exception cref="ConcurrencyException">
    /// Throws when there is a concurrency related conflict occured during the operation.
    /// </exception>
    /// <exception cref="OperationException">
    /// Throws under the following circumstances:<br/>
    /// - The <see cref="Seating"/> specified by the value of <c>SeatingId</c> property in
    /// <paramref name="orderDto"/> does't exist.<br/>
    /// - The <see cref="MenuItem"/> specified by the value of <c>Items[i].MenuItemId</c>
    /// property in <paramref name="orderDto"/> doesn't exist.<br/>
    /// </exception>
    Task<int> CreateAsync(OrderUpsertDto orderDto);

    /// <summary>
    /// Updates an existing order, specified by its id.
    /// </summary>
    /// <param name="id">
    /// The id of the order to update.
    /// </param>
    /// <param name="orderDto">
    /// A DTO containing the data for the updating operation.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation.
    /// </returns>
    /// <exception cref="ConcurrencyException">
    /// Throws when there is a concurrency-related conflict occured during the operation.
    /// </exception>
    /// <exception cref="NotFoundException">
    /// Throws when the order specified by <paramref name="id"/> doesn't exist.
    /// </exception>
    /// <exception cref="OperationException">
    /// Throws under the following circumstances:<br/>
    /// - The <see cref="Seating"/> specified by the value of the property <c>SeatingId</c> in
    /// <paramref name="orderDto"/> doesn't exist.<br/>
    /// - The <see cref="MenuItem"/> specified by the value of the property
    /// <c>Items[i].MenuItemId</c> in <paramref name="orderDto"/> doesn't exist.
    /// </exception>
    /// <exception cref="ConcurrencyException"></exception>
    Task UpdateAsync(int id, OrderUpsertDto orderDto);

    /// <summary>
    /// Deletes an existing order, specified by its id.
    /// </summary>
    /// <param name="id">
    /// The id of the order to delete.KJ
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation.
    /// </returns>
    /// <exception cref="NotFoundException">
    /// Throws when the order specified by <paramref name="id"/> doesn't exist.
    /// </exception>
    Task DeleteAsync(int id);
}
