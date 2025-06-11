using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using QuanNhauSanVuon.Dtos;
using QuanNhauSanVuon.Services.Interfaces;

namespace QuanNhauSanVuon.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IValidator<SignInDto> _signInValidator;

    public AuthenticationController(
            IAuthenticationService authenticationService,
            IValidator<SignInDto> signInValidator)
    {
        _authenticationService = authenticationService;
        _signInValidator = signInValidator;
    }

    [HttpPost("GetAccessCookie")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> GetAccessCookie([FromBody] SignInDto dto)
    {
        dto.TransformValues();
        _signInValidator.ValidateAndThrow(dto);

        int userId = await _authenticationService.SignInAsync(dto);
        return Ok(userId);
    }

    [HttpPost("ClearAccessCookie")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> ClearAccessCookie()
    {
        await _authenticationService.SignOutAsync();
        return Ok();
    }

    [HttpGet("CheckAuthenticationStatus")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult CheckAuthenticationStatus()
    {
        return Ok();
    }
}