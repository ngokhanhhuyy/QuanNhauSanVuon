using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MySqlConnector;
using QuanNhauSanVuon.Dtos;
using QuanNhauSanVuon.Localization;
using QuanNhauSanVuon.Services.Entities;
using QuanNhauSanVuon.Services.Exceptions;
using QuanNhauSanVuon.Services.Interfaces;
using QuanNhauSanVuon.Services.Structs;

namespace QuanNhauSanVuon.Services;

/// <inheritdoc/>
public class SeatingAreaService : ISeatingAreaService
{
    private readonly DatabaseContext _context;
    private readonly MySqlExceptionHandler _exceptionHandler;

    public SeatingAreaService(
            DatabaseContext context,
            MySqlExceptionHandler exceptionHandler)
    {
        _context = context;
        _exceptionHandler = exceptionHandler;
    }

    /// <inheritdoc/>
    public async Task<List<SeatingAreaDto>> GetAllAsync()
    {
        return await _context.SeatingAreas
            .Include(sa => sa.Seatings)
            .OrderBy(sa => sa.Id)
            .Select(sa => new SeatingAreaDto(sa))
            .ToListAsync();
    }
    
    /// <inheritdoc/>
    public async Task<SeatingAreaDto> GetSingleAsync(int id)
    {
        return await _context.SeatingAreas
            .Include(sa => sa.Seatings)
            .Where(sa => sa.Id == id)
            .Select(sa => new SeatingAreaDto(sa))
            .SingleOrDefaultAsync()
            ?? throw new NotFoundException();
    }

    /// <inheritdoc/>
    public async Task<int> CreateAsync(SeatingAreaUpsertDto dto)
    {
        await using IDbContextTransaction transaction = await _context.Database
            .BeginTransactionAsync();

        List<SeatingArea> existingSeatingAreas = await _context.SeatingAreas.ToListAsync();
        List<Point> existingPoints = existingSeatingAreas
            .SelectMany(sa => sa.TakenUpPositions)
            .Distinct()
            .ToList();
        bool hasOverlappedPoints = existingPoints.Any(p => dto.TakenUpPositions.Contains(p));
        if (hasOverlappedPoints)
        {
            throw new OperationException(
                new[] { nameof(dto.TakenUpPositions) },
                ErrorMessages.SeatingAreaPositionsOverlapped);
        }

        object[] uniquePropertyPathElements = null;

        try
        {
            uniquePropertyPathElements = [nameof(dto.Name)];
            SeatingArea seatingArea = new SeatingArea
            {
                Name = dto.Name,
                Color = dto.Color,
                TakenUpPositions = dto.TakenUpPositions,
                Seatings = new List<Seating>()
            };

            _context.SeatingAreas.Add(seatingArea);
            await _context.SaveChangesAsync();

            if (dto.Seatings != null)
            {
                for (int index = 0; index < dto.Seatings.Count; index++)
                {
                    SeatingUpsertDto seatingDto = dto.Seatings[index];
                    if (!seatingDto.HasBeenChanged || seatingDto.HasBeenDeleted)
                    {
                        continue;
                    }

                    uniquePropertyPathElements = new object[] {
                        nameof(dto.Seatings),
                        index,
                        nameof(seatingDto.Name)
                    };

                    Seating seating = new Seating
                    {
                        Name = seatingDto.Name,
                        PositionX = seatingDto.PositionX,
                        PositionY = seatingDto.PositionY
                    };

                    seatingArea.Seatings.Add(seating);
                    _context.Seatings.Add(seating);
                    await _context.SaveChangesAsync();
                }
            }

            await transaction.CommitAsync();
            return seatingArea.Id;
        }
        catch (DbUpdateException exception)
        {
            bool isExceptionConvertible = TryConvertingDbUpdateException(
                exception,
                uniquePropertyPathElements,
                out OperationException convertedException);

            if (isExceptionConvertible)
            {
                throw convertedException;
            }

            throw;
        }
    }

    /// <inheritdoc/>
    public async Task UpdateAsync(int id, SeatingAreaUpsertDto dto)
    {
        SeatingArea seatingArea = await _context.SeatingAreas
            .Include(sa => sa.Seatings)
            .SingleOrDefaultAsync(sa => sa.Id == id)
            ?? throw new NotFoundException();

        await using IDbContextTransaction transaction = await _context.Database
            .BeginTransactionAsync();
        object[] uniquePropertyPathElements = new[] { nameof(dto.Name) };

        try
        {
            seatingArea.Name = dto.Name;
            seatingArea.Color = dto.Color;
            seatingArea.TakenUpPositions = dto.TakenUpPositions;

            await _context.SaveChangesAsync();

            dto.Seatings ??= new List<SeatingUpsertDto>();
            seatingArea.Seatings ??= new List<Seating>();
            for (int index = 0; index < dto.Seatings.Count; index++)
            {
                SeatingUpsertDto seatingDto = dto.Seatings[index];
                Seating seating;
                uniquePropertyPathElements = new object[]
                {
                    nameof(dto.Seatings),
                    index,
                    nameof(dto.Name)
                };

                if (!seatingDto.Id.HasValue)
                {
                    seating = new Seating
                    {
                        Name = seatingDto.Name,
                        PositionX = seatingDto.PositionX,
                        PositionY = seatingDto.PositionY
                    };

                    seatingArea.Seatings.Add(seating);
                }
                else
                {
                    seating = seatingArea.Seatings
                        .SingleOrDefault(s => s.Id == seatingDto.Id.Value)
                        ?? throw OperationException.NotFound(
                            new object[] {
                                nameof(dto.Seatings),
                                index,
                                nameof(seatingDto.Id)
                            },
                            DisplayNames.Seating);
                    
                    if (seatingDto.HasBeenDeleted)
                    {
                        seatingArea.Seatings.Remove(seating);
                    }
                    else if (seatingDto.HasBeenChanged)
                    {
                        seating.Name = seatingDto.Name;
                        seating.PositionX = seatingDto.PositionX;
                        seating.PositionY = seatingDto.PositionY;
                    }
                }

                await _context.SaveChangesAsync();
            }

            await transaction.CommitAsync();
        }
        catch (DbUpdateException exception)
        {
            bool isExceptionConvertible = TryConvertingDbUpdateException(
                exception,
                uniquePropertyPathElements,
                out OperationException convertedException);

            if (isExceptionConvertible)
            {
                throw convertedException;
            }

            throw;
        }
    }

    /// <inheritdoc/>
    public async Task DeleteAsync(int id)
    {
        SeatingArea seatingArea = await _context.SeatingAreas
            .Where(sa => sa.Id == id)
            .SingleOrDefaultAsync()
            ?? throw new NotFoundException();

        try
        {
            int deletedRecordCount = await _context.SeatingAreas
            .Where(sa => sa.Id == id)
            .ExecuteDeleteAsync();
        }
        catch (MySqlException exception)
        {
            MySqlExceptionHandledResult handledResult = _exceptionHandler.Handle(exception);
            if (handledResult.IsDeleteOrUpdateRestricted)
            {
                throw OperationException.DeleteRestricted(
                    new[] { nameof(SeatingArea.Seatings) },
                    DisplayNames.SeatingArea);
            }

            throw;
        }
    }

    /// <summary>
    /// Tries converting the exception thrown by the database during the creating or updating
    /// operation into the exception that describes the error by convention.
    /// </summary>
    /// <param name="exception">
    /// The exception instance thrown by the database during the operation.
    /// </param>
    /// <param name="uniquePropertyPathElements">
    /// An <see cref="Array"/> of property path elements, representing the address of the
    /// property on the DTO where the unique constraint is violated (if violated).
    /// </param>
    /// <param name="convertedException">
    /// The converted exception if the conversion is successful. Otherwise, <c>null</c>.
    /// </param>
    /// <returns>
    /// A <see cref="bool"/> value indicates whether the conversion is successful.
    /// </returns>
    private bool TryConvertingDbUpdateException(
        DbUpdateException exception,
        object[] uniquePropertyPathElements,
        out OperationException convertedException)
    {
        convertedException = null;
        if (exception.InnerException is not MySqlException mySqlException)
        {
            return false;
        }

        MySqlExceptionHandledResult result = _exceptionHandler.Handle(mySqlException);

        if (result.IsUniqueConstraintViolated)
        {
            convertedException = OperationException.Duplicated(uniquePropertyPathElements);
            return true;
        }

        return false;
    }
}