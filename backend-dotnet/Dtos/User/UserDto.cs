using QuanNhauSanVuon.Services.Entities;

namespace QuanNhauSanVuon.Dtos;

public class UserDto
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public RoleDto Role { get; set; }

    public UserDto(User user)
    {
        Id = user.Id;
        UserName = user.UserName;
        Role = new RoleDto(user.Role);
    }
}