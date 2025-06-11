using QuanNhauSanVuon.Dtos.Interfaces;
using QuanNhauSanVuon.Extensions;

namespace QuanNhauSanVuon.Dtos;

public class MenuItemUpsertDto : IRequestDto
{
    public string Name { get; set; }
    public long DefaultNetPrice { get; set; }
    public int DefaultVatPercentage { get; set; }
    public string Unit { get; set; }
    public string ThumbnailUrl { get; set; }
    public int? CategoryId { get; set; }

    public void TransformValues()
    {
        Name = Name?.ToNullIfEmpty();
        Unit = Unit?.ToNullIfEmpty();
        ThumbnailUrl = ThumbnailUrl?.ToNullIfEmpty();
        if (CategoryId == 0)
        {
            CategoryId = null;
        }
    }
}