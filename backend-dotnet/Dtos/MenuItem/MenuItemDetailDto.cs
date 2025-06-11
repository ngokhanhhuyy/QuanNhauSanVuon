using QuanNhauSanVuon.Services.Entities;

namespace QuanNhauSanVuon.Dtos;

public class MenuItemDetailDto : MenuItemBasicDto
{
    public DateTime CreatedDateTime { get; set; }
    public DateTime? LastUpdatedDateTime { get; set; }
    public MenuCategoryDto Category { get; set; }

    public MenuItemDetailDto(MenuItem menuItem) : base(menuItem)
    {
        CreatedDateTime = menuItem.CreatedDateTime;
        LastUpdatedDateTime = menuItem.LastUpdatedDateTime;
        if (menuItem.Category != null)
        {
            Category = new MenuCategoryDto(menuItem.Category);
        }
    }
}