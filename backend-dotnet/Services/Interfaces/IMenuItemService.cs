using QuanNhauSanVuon.Dtos;
using QuanNhauSanVuon.Services.Entities;
using QuanNhauSanVuon.Services.Exceptions;

namespace QuanNhauSanVuon.Services.Interfaces;

/// <summary>
/// A service to handle the operations which are related to menu items.
/// </summary>
public interface IMenuItemService
{
    /// <summary>
    /// Gets a list of menu items with the specified filtering and paginating conditions.
    /// </summary>
    /// <param name="listDto">
    /// A DTO containing the conditions for the results.
    /// </param>
    /// <returns>
    /// A <see cref="Task{T}"/> representing the asynchronous operation, which result is the
    /// list DTO containing the results and additional information for pagination. 
    /// </returns>
    Task<MenuItemListDto> GetListAsync(MenuItemListFiltersDto listDto);

    /// <summary>
    /// Gets an existing menu item, specified by its id.
    /// </summary>
    /// <param name="id">
    /// The id of the menu item to retrieve.
    /// </param>
    /// <returns>
    /// A <see cref="Task{T}"/> representing the asynchronous operation, which result is the
    /// DTO containing the information of the menu item.
    /// </returns>
    /// <exception cref="NotFoundException">
    /// Throws when the menu item, specified by <paramref name="id"/> doesn't exist.
    /// </exception>
    Task<MenuItemDetailDto> GetDetailAsync(int id);

    /// <summary>
    /// Creates a new menu item.
    /// </summary>
    /// <param name="upsertDto">
    /// A DTO containing the data for the creating operation.
    /// </param>
    /// <returns>
    /// A <see cref="Task{T}"/> representing the asynchronous operation, which result is the id
    /// of the created menu item. 
    /// </returns>
    /// <exception cref="OperationException">
    /// Throws when under the following circumstances:<br/>
    /// - The value of <c>Name</c> property in <paramref name="upsertDto"/> is already in use.
    /// <br/>
    /// - The <see cref="MenuCategory"/> specified by the value of the <c>CategoryId</c> in
    /// <paramref name="upsertDto"/> doesn't exist.
    /// </exception>
    Task<int> CreateAsync(MenuItemUpsertDto upsertDto);

    /// <summary>
    /// Updates an existing menu item, specified by its id.
    /// </summary>
    /// <param name="id">
    /// The id of the menu item to update.
    /// </param>
    /// <param name="upsertDto">
    /// The DTO containing the data for the updating operation.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation.
    /// </returns>
    /// <exception cref="NotFoundException">
    /// Throws when the menu item, specified by <paramref name="id"/> doesn't exist.
    /// </exception>
    /// <exception cref="OperationException">
    /// Throws when under the following circumstances:<br/>
    /// - The value of <c>Name</c> property in <paramref name="upsertDto"/> is already in use.
    /// <br/>
    /// - The <see cref="MenuCategory"/> specified by the value of the <c>CategoryId</c> in
    /// <paramref name="upsertDto"/> doesn't exist.
    /// </exception>
    Task UpdateAsync(int id, MenuItemUpsertDto upsertDto);

    /// <summary>
    /// Deletes an existing menu item, specified by its id.
    /// </summary>
    /// <remarks>
    /// If the menu item is related to a menu category id and the deletion is restricted, this
    /// will execute a soft-delete operation instead.
    /// </remarks>
    /// <param name="id">
    /// The id of the menu item to delete.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation.
    /// </returns>
    /// <exception cref="NotFoundException">
    /// Throws when the menu item, specified by <paramref name="id"/> doesn't exist.
    /// </exception>
    Task DeleteAsync(int id);
}
