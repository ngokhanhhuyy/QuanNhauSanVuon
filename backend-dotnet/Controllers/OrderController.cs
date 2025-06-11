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
public class OrderController : ControllerBase
{
    private readonly IOrderService _service;
    private readonly IValidator<OrderListFiltersDto> _listFiltersValidator;
    private readonly IValidator<OrderUpsertDto> _upsertValidator;

    public OrderController(
        IOrderService service,
        IValidator<OrderListFiltersDto> listFiltersValidator,
        IValidator<OrderUpsertDto> upsertValidator)
    {
        _service = service;
        _listFiltersValidator = listFiltersValidator;
        _upsertValidator = upsertValidator;
    }

    [HttpGet]
    [ProducesResponseType<OrderListDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<List<PropertyErrorDetail>>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> List([FromQuery] OrderListFiltersDto dto)
    {
        dto.TransformValues();
        _listFiltersValidator.ValidateAndThrow(dto);

        return Ok(await _service.GetListAsync(dto));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType<OrderListDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Detail(int id)
    {
        return Ok(await _service.GetDetailAsync(id));
    }

    [HttpPost]
    [ProducesResponseType<int>(StatusCodes.Status201Created)]
    [ProducesResponseType<List<PropertyErrorDetail>>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType<List<PropertyErrorDetail>>(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Create(OrderUpsertDto dto)
    {
        dto.TransformValues();
        _upsertValidator.ValidateAndThrow(dto);

        int createdId = await _service.CreateAsync(dto);
        string createdUrl = Url.Action("Detail", new { id = createdId });
        return Created(createdUrl, createdId);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType<int>(StatusCodes.Status200OK)]
    [ProducesResponseType<List<PropertyErrorDetail>>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType<List<PropertyErrorDetail>>(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Update(int id, [FromBody] OrderUpsertDto dto)
    {
        dto.TransformValues();
        _upsertValidator.ValidateAndThrow(dto);

        await _service.UpdateAsync(id, dto);
        return Ok();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType<int>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        return Ok();
    }
}