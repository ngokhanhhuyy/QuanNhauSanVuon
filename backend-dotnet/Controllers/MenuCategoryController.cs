using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using QuanNhauSanVuon.Dtos;
using QuanNhauSanVuon.Services.Exceptions;
using QuanNhauSanVuon.Services.Interfaces;

namespace QuanNhauSanVuon.Controllers;

[Route("api/[controller]")]
[Authorize]
[ApiController]
public class MenuCategoryController : ControllerBase
{
    private readonly IMenuCategoryService _service;
    private readonly IValidator<MenuCategoryUpsertDto> _validator;

    public MenuCategoryController(
            IMenuCategoryService service,
            IValidator<MenuCategoryUpsertDto> validator)
    {
        _service = service;
        _validator = validator;
    }

    [HttpGet]
    [ProducesResponseType<List<MenuCategoryDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> All()
    {
        return Ok(await _service.GetAllAsync());
    }


    [HttpGet("{id:int}")]
    [ProducesResponseType<MenuCategoryDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Single(int id)
    {
        return Ok(await _service.GetSingleAsync(id));
    }

    [HttpPost]
    [ProducesResponseType<int>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Create(MenuCategoryUpsertDto dto)
    {
        dto.TransformValues();
        _validator.ValidateAndThrow(dto);

        int createdId = await _service.CreateAsync(dto);
        string createdUrl = Url.Action("Single", new { id = createdId });

        return Created(createdUrl, createdId);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Update(int id, [FromBody] MenuCategoryUpsertDto dto)
    {
        dto.TransformValues();
        _validator.ValidateAndThrow(dto);

        await _service.UpdateAsync(id, dto);
        return Ok();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _service.DeleteAsync(id);
            return Ok();
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }
}
