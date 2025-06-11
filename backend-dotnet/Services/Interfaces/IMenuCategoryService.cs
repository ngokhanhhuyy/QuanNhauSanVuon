using QuanNhauSanVuon.Dtos;
using QuanNhauSanVuon.Services.Exceptions;

namespace QuanNhauSanVuon.Services.Interfaces;

public interface IMenuCategoryService
{
    /// <summary>
    /// Gets all menu categories.
    /// </summary>
    /// <returns>
    /// A <see cref="Task{T}"/> representing the asynchronous operation, which result is a
    /// <see cref="List{T}"/> of DTOs, containing the information of the menu categories. 
    /// </returns>
    Task<List<MenuCategoryDto>> GetAllAsync();

    /// <summary>
    /// Gets a specific menu category, specified by its id.
    /// </summary>
    /// <param name="id">
    /// The id of the menu category to retrieve.
    /// </param>
    /// <returns>
    /// A <see cref="Task{T}"/> representing the asynchonous operation, which result is the DTO
    /// containing the information of the menu category.
    /// </returns>
    /// <exception cref="NotFoundException">
    /// Throws when the menu category specified by <paramref name="id"/> doesn't exist.
    /// </exception>
    Task<MenuCategoryDto> GetSingleAsync(int id);

    /// <summary>
    /// Creates a new menu category.
    /// </summary>
    /// <param name="requestDto">
    /// A DTO containing the data for the creating operation.
    /// </param>
    /// <returns>
    /// A <see cref="Task{T}"/> representing the asynchronous operation, which result is the id
    /// of the created menu category. 
    /// </returns>
    /// <exception cref="OperationException">
    /// Throws when the name specified in the <paramref name="requestDto"/> already exists.
    /// </exception>
    Task<int> CreateAsync(MenuCategoryUpsertDto requestDto);

    /// <summary>
    /// Updates an existing menu category, specified by its id.
    /// </summary>
    /// <param name="id">
    /// The id of the menu category to update.
    /// </param>
    /// <param name="requestDto">
    /// A DTO containing the data for the updating operation.
    /// </param>
    /// <returns>
    /// A <see cref="Task{T}"/> representing the asynchronous operation. 
    /// </returns>
    /// <exception cref="NotFoundException">
    /// Throws when the menu category, specified by <paramref name="id"/> doesn't exist.
    /// </exception>
    /// <exception cref="OperationException">
    /// Throws when the name specified in the <paramref name="requestDto"/> already exists.
    /// </exception>
    Task UpdateAsync(int id, MenuCategoryUpsertDto requestDto);

    /// <summary>
    /// Deletes an existing menu category, specified by its id.
    /// </summary>
    /// <param name="id">
    /// The id of the menu category to delete.
    /// </param>
    /// <returns>
    /// A <see cref="Task{T}"/> representing the asynchronous operation. 
    /// </returns>
    /// <exception cref="NotFoundException">
    /// Throws when the menu category, specified by <paramref name="id"/> doesn't exist.
    /// </exception>
    Task DeleteAsync(int id);
}
