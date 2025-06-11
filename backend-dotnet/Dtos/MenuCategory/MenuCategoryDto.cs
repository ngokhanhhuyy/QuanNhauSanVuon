using QuanNhauSanVuon.Services.Entities;

namespace QuanNhauSanVuon.Dtos;

public class MenuCategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; }

    public MenuCategoryDto(MenuCategory menuCategory)
    {
        Id = menuCategory.Id;
        Name = menuCategory.Name;
    }
}