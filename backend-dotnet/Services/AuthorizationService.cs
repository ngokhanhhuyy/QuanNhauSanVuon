using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using QuanNhauSanVuon.Dtos;
using QuanNhauSanVuon.Services.Entities;
using QuanNhauSanVuon.Services.Exceptions;
using QuanNhauSanVuon.Services.Interfaces;

namespace QuanNhauSanVuon.Services;

public class AuthorizationService : IAuthorizationService
{
    private readonly DatabaseContext _context;
    private User _user;

    public AuthorizationService(
            DatabaseContext context,
            IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        ClaimsPrincipal user = httpContextAccessor.HttpContext?.User;
        
        string userIdAsString = user?.FindFirstValue(ClaimTypes.NameIdentifier);
        bool parsable = int.TryParse(userIdAsString, out int userId);
        if (!parsable)
        {
            throw new AuthenticationException();
        }
        
        SetUserId(userId);
    }

    public void SetUserId(int id)
    {
        _user = _context.Users
            .Include(u => u.Roles).ThenInclude(r => r.Claims)
            .Single(u => u.Id == id);
    }
    
    public int GetUserId()
    {
        return _user.Id;
    }

    public UserDto GetUserDetail()
    {
        return new UserDto(_user);
    }
}