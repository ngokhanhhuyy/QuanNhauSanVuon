using QuanNhauSanVuon.Dtos.Interfaces;
using QuanNhauSanVuon.Extensions;

namespace QuanNhauSanVuon.Dtos;

public class SeatingUpsertDto : IRequestDto
{
    public int? Id { get; set; }
    public string Name { get; set; }
    public PointDto Position { get; set; }
    public int? AreaId { get; set; }
    public bool HasBeenChanged { get; set; }
    public bool HasBeenDeleted { get; set; }

    public void TransformValues()
    {
        Name = Name?.ToNullIfEmpty();
    }
}