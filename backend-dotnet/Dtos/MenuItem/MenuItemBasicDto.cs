using QuanNhauSanVuon.Services.Entities;

namespace QuanNhauSanVuon.Dtos;

public class MenuItemBasicDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public long DefaultNetPrice { get; set; }
    public int DefaultVatPercentage { get; set; }
    public string Unit { get; set; }
    public string ThumbnailUrl { get; set; }

    public MenuItemBasicDto(MenuItem menuItem)
    {
        Id = menuItem.Id;
        Name = menuItem.Name;
        DefaultNetPrice = menuItem.DefaultNetPrice;
        DefaultVatPercentage = menuItem.DefaultVatPercentage;
        Unit = menuItem.Unit;
        ThumbnailUrl = menuItem.ThumbnailUrl;
    }
}