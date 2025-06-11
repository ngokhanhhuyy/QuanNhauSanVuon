using QuanNhauSanVuon.Dtos;
using QuanNhauSanVuon.Services.Exceptions;

namespace QuanNhauSanVuon.Services.Interfaces;

/// <summary>
/// A service to handle operations which are related to seating areas.
/// </summary>
public interface ISeatingAreaService
{
    /// <summary>
    /// Gets all seating areas' information.
    /// </summary>
    /// <returns>
    /// A <see cref="Task{T}"/> representing the asynchronous operation, which result is a
    /// <see cref="List{T}"/> of DTOs, containing the information of the seating areas.
    /// </returns>
    Task<List<SeatingAreaDto>> GetAllAsync();

    /// <summary>
    /// Gets a single existing seating area.
    /// </summary>
    /// <param name="id">
    /// The id of the seating area to retrieve.
    /// </param>
    /// <returns>
    /// A <see cref="Task{T}"/> representing the asynchronous operation, which result is a DTO
    /// containing the information of the seating area.
    /// </returns>
    /// <exception cref="NotFoundException">
    /// Throws when the seating area with the id specified by <paramref name="id"/> doesn't
    /// exist.
    /// </exception>
    Task<SeatingAreaDto> GetSingleAsync(int id);

    /// <summary>
    /// Creates a new seating area.
    /// </summary>
    /// <param name="dto">
    /// A DTO containing the data for the creating operation.
    /// </param>
    /// <returns>
    /// A <see cref="Task{T}"/> representing the asynchronous operation, which result is the id
    /// of the created seating area.
    /// </returns>
    /// <exception cref="OperationException">
    /// Throws under follow circustances:
    /// - When the name specified in <paramref name="dto"/> already exists.
    /// - When some seating, specified by its id, doesn't exist.
    /// - When some seating having the name that already exists.
    /// </exception>
    Task<int> CreateAsync(SeatingAreaUpsertDto dto);

    /// <summary>
    /// Update an existing seating area, specified by its id.
    /// </summary>
    /// <param name="id">
    /// The id of the seating area to update.
    /// </param>
    /// <param name="dto">
    /// A DTO containing the data for the updating operation.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation.
    /// </returns>
    /// <exception cref="NotFoundException">
    /// Throws when the seating area specified by <paramref name="id"/> doesn't exist.
    /// </exception>
    /// <exception cref="OperationException">
    /// Throws under follow circustances:
    /// - When the name specified in <paramref name="dto"/> already exists.
    /// - When some seating, specified by its id, doesn't exist.
    /// - When some seating having the name that already exists.
    /// </exception>
    Task UpdateAsync(int id, SeatingAreaUpsertDto dto);

    /// <summary>
    /// Deletes an existing seating area.
    /// </summary>
    /// <param name="id">
    /// The id of the seating area to delete.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation.
    /// </returns>
    /// <exception cref="NotFoundException">
    /// Throws when the seating area specified by <paramref name="id"/> doesn't exist.
    /// </exception>
    Task DeleteAsync(int id);
}