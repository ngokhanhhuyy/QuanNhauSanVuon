using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QuanNhauSanVuon.Dtos;
using QuanNhauSanVuon.Extensions;
using QuanNhauSanVuon.Localization;
using QuanNhauSanVuon.Services.Entities;
using QuanNhauSanVuon.Services.Exceptions;
using QuanNhauSanVuon.Services.Interfaces;

namespace QuanNhauSanVuon.Services;

/// <inheritdoc cref="IAuthenticationService" />
internal class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public AuthenticationService(
            UserManager<User> userManager,
            SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    /// <inheritdoc />
    public async Task<int> SignInAsync(SignInDto dto)
    {
        // Check if user exists.
        User user = await _userManager.Users
            .Include(u => u.Roles).ThenInclude(r => r.Claims)
            .AsSplitQuery()
            .SingleOrDefaultAsync(u => u.UserName == dto.UserName && !u.IsDeleted);

        string errorMessage;
        if (user == null)
        {
            errorMessage = ErrorMessages.NotFoundByProperty
                .ReplaceResourceName(DisplayNames.User)
                .ReplacePropertyName(DisplayNames.UserName)
                .ReplaceAttemptedValue(dto.UserName);
            throw new OperationException(new[] { nameof(dto.UserName) }, errorMessage);
        }

        // Check the password.
        SignInResult signInResult = await _signInManager
            .CheckPasswordSignInAsync(user, dto.Password, lockoutOnFailure: false);
        if (!signInResult.Succeeded)
        {
            errorMessage = ErrorMessages.Incorrect.ReplacePropertyName(DisplayNames.Password);
            throw new OperationException(new[] { nameof(dto.Password) }, errorMessage);
        }

        // Prepare the claims to be added in to the generating cookie.
        List<Claim> claims =
        [
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName!),
            new Claim(ClaimTypes.Role, user.Role.Name!),
            .. user.Role.Claims.Select(c => new Claim("Permission", c.ClaimValue!))
        ];
        claims.AddRange(user.Role.Claims
            .Where(c => c.ClaimType == "Permission")
            .Select(c => new Claim("Permission", c.ClaimValue!)));

        // Perform sign in operation.
        await _signInManager.SignInWithClaimsAsync(user, false, claims);

        return user.Id;
    }

    /// <inheritdoc />
    public async Task SignOutAsync()
    {
        await _signInManager.SignOutAsync();
    }
}