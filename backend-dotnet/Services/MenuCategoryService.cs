using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using QuanNhauSanVuon.Dtos;
using QuanNhauSanVuon.Services.Entities;
using QuanNhauSanVuon.Services.Exceptions;
using QuanNhauSanVuon.Services.Interfaces;

namespace QuanNhauSanVuon.Services;

/// <inheritdoc />
public class MenuCategoryService : IMenuCategoryService
{
    private readonly DatabaseContext _context;
    private readonly MySqlExceptionHandler _exceptionHandler;

    public MenuCategoryService(
            DatabaseContext context,
            MySqlExceptionHandler exceptionHandler)
    {
        _context = context;
        _exceptionHandler = exceptionHandler;
    }

    /// <inheritdoc />
    public async Task<List<MenuCategoryDto>> GetAllAsync()
    {
        return await _context.MenuCategories
            .OrderBy(menuCategory => menuCategory.Id)
            .Select(menuCategory => new MenuCategoryDto(menuCategory))
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<MenuCategoryDto> GetSingleAsync(int id)
    {
        return await _context.MenuCategories
            .Where(menuCategory => menuCategory.Id == id)
            .Select(menuCategory => new MenuCategoryDto(menuCategory))
            .FirstOrDefaultAsync()
            ?? throw new NotFoundException();
    }

    /// <inheritdoc />
    public async Task<int> CreateAsync(MenuCategoryUpsertDto requestDto)
    {
        // Initialize the entity.
        MenuCategory menuCategory = new MenuCategory
        {
            Name = requestDto.Name
        };

        _context.MenuCategories.Add(menuCategory);

        try
        {
            await _context.SaveChangesAsync();
            return menuCategory.Id;
        }
        catch (DbUpdateException exception)
        {
            if (exception.InnerException is not MySqlException mySqlException)
            {
                throw;
            }
            
            MySqlExceptionHandledResult result = _exceptionHandler.Handle(mySqlException);
            if (result.IsUniqueConstraintViolated)
            {
                throw OperationException.Duplicated(nameof(MenuCategory.Name));
            }

            throw;
        }
    }

    /// <inheritdoc />
    public async Task UpdateAsync(int id, MenuCategoryUpsertDto requestDto)
    {
        try
        {
            int updatedRecordCount = await _context.MenuCategories
                .Where(menuCategory => menuCategory.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(menuCategory => menuCategory.Name, requestDto.Name));

            if (updatedRecordCount == 0)
            {
                throw new NotFoundException();
            }
        }
        catch (DbUpdateException exception)
        {
            if (exception.InnerException is not MySqlException mySqlException)
            {
                throw;
            }

            MySqlExceptionHandledResult result = _exceptionHandler.Handle(mySqlException);

            if (result.IsUniqueConstraintViolated)
            {
                throw OperationException.Duplicated(nameof(MenuCategory.Name));
            }

            throw;
        }
    }

    /// <inheritdoc />
    public async Task DeleteAsync(int id)
    {
        int deletedRecordCount = await _context.MenuCategories
            .Where(menuCategory => menuCategory.Id == id)
            .ExecuteDeleteAsync();

        if (deletedRecordCount == 0)
        {
            throw new NotFoundException();
        }
    }
}
