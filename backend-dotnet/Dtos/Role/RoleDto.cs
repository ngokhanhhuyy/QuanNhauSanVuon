using QuanNhauSanVuon.Services.Entities;

namespace QuanNhauSanVuon.Dtos;

public class RoleDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string DisplayName { get; set; }
    public int PowerLevel { get; set; }

    public RoleDto(Role role)
    {
        Id = role.Id;
        Name = role.Name;
        DisplayName = role.DisplayName;
        PowerLevel = role.PowerLevel;
    }
}