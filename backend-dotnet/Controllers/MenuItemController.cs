using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using QuanNhauSanVuon.Dtos;
using QuanNhauSanVuon.Services.Interfaces;
using QuanNhauSanVuon.Services.Exceptions;

namespace QuanNhauSanVuon.Controllers;

[Route("api/[controller]")]
[Authorize]
[ApiController]
public class MenuItemController : ControllerBase
{
    private readonly IMenuItemService _service;
    private readonly IValidator<MenuItemListFiltersDto> _listValidator;
    private readonly IValidator<MenuItemUpsertDto> _upsertValidator;

    public MenuItemController(
        IMenuItemService service,
        IValidator<MenuItemListFiltersDto> listValidator,
        IValidator<MenuItemUpsertDto> upsertValidator)
    {
        _service = service;
        _listValidator = listValidator;
        _upsertValidator = upsertValidator;
    }

    [HttpGet]
    [ProducesResponseType<MenuItemListDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<List<PropertyErrorDetail>>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> List([FromQuery] MenuItemListFiltersDto dto)
    {
        dto.TransformValues();
        _listValidator.ValidateAndThrow(dto);

        return Ok(await _service.GetListAsync(dto));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType<MenuItemListDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Detail(int id)
    {
        return Ok(await _service.GetDetailAsync(id));
    }

    [HttpPost]
    [ProducesResponseType<int>(StatusCodes.Status201Created)]
    [ProducesResponseType<List<PropertyErrorDetail>>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<List<PropertyErrorDetail>>(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Create(MenuItemUpsertDto dto)
    {
        dto.TransformValues();
        _upsertValidator.ValidateAndThrow(dto);

        int createdId = await _service.CreateAsync(dto);
        string createdUrl = Url.Action("Detail", new { id = createdId });
        return Created(createdUrl, createdId);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<List<PropertyErrorDetail>>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType<List<PropertyErrorDetail>>(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Update(int id, MenuItemUpsertDto dto)
    {
        dto.TransformValues();
        _upsertValidator.ValidateAndThrow(dto);

        await _service.UpdateAsync(id, dto);
        return Ok();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        return Ok();
    }
}