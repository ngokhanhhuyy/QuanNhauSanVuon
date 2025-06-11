using QuanNhauSanVuon.Dtos.Interfaces;
using QuanNhauSanVuon.Extensions;

namespace QuanNhauSanVuon.Dtos;

public class MenuCategoryUpsertDto : IRequestDto
{
    public required string Name { get; set; }

    public void TransformValues()
    {
        Name = Name.ToNullIfEmpty();
    }
}
