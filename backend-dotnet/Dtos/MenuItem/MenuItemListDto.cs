using QuanNhauSanVuon.Services.Entities;

namespace QuanNhauSanVuon.Dtos;

public class MenuItemListDto
{
    public int PageCount { get; set; }
    public List<MenuItemBasicDto> Items { get; set; } = new List<MenuItemBasicDto>();

    public MenuItemListDto() { }

    public MenuItemListDto(int pageCount, List<MenuItem> menuItems)
    {
        PageCount = pageCount;
        Items = menuItems.Select(mi => new MenuItemBasicDto(mi)).ToList();
    }
}