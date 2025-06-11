using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using FluentValidation.Results;
using QuanNhauSanVuon.Dtos;
using QuanNhauSanVuon.Extensions;
using QuanNhauSanVuon.Services.Exceptions;
using QuanNhauSanVuon.Services.Interfaces;

namespace QuanNhauSanVuon.Controllers;

[Route("api/[controller]")]
[Authorize]
[ApiController]
public class SeatingAreaController : ControllerBase
{
    private readonly ISeatingAreaService _service;
    private readonly IValidator<SeatingAreaUpsertDto> _upsertValidator;

    public SeatingAreaController(
        ISeatingAreaService service,
        IValidator<SeatingAreaUpsertDto> upsertValidator)
    {
        _service = service;
        _upsertValidator = upsertValidator;
    }

    [HttpGet]
    [ProducesResponseType<List<SeatingAreaDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<List<SeatingAreaDto>>(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> All()
    {
        return Ok(await _service.GetAllAsync());
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType<List<SeatingAreaDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<List<SeatingAreaDto>>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<List<SeatingAreaDto>>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Single(int id)
    {
        return Ok(await _service.GetSingleAsync(id));
    }

    [HttpPost]
    [ProducesResponseType<List<SeatingAreaDto>>(StatusCodes.Status201Created)]
    [ProducesResponseType<List<SeatingAreaDto>>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<List<SeatingAreaDto>>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<List<SeatingAreaDto>>(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Create([FromBody] SeatingAreaUpsertDto dto)
    {
        dto.TransformValues();
        _upsertValidator.ValidateAndThrow(dto);

        int createdId = await _service.CreateAsync(dto);
        string createdUrl = Url.Action("Single", new { id = createdId });
        return Created(createdUrl, createdId);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType<List<SeatingAreaDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<List<SeatingAreaDto>>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<List<SeatingAreaDto>>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<List<SeatingAreaDto>>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<List<SeatingAreaDto>>(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Update(int id, [FromBody] SeatingAreaUpsertDto dto)
    {
        dto.TransformValues();
        _upsertValidator.ValidateAndThrow(dto);

        await _service.UpdateAsync(id, dto);
        return Ok();
    }

    [ProducesResponseType<List<SeatingAreaDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<List<SeatingAreaDto>>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<List<SeatingAreaDto>>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<List<SeatingAreaDto>>(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        return Ok();
    }
}