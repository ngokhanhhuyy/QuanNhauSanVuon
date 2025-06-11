using QuanNhauSanVuon.Dtos.Interfaces;
using QuanNhauSanVuon.Extensions;
using QuanNhauSanVuon.Services.Structs;

namespace QuanNhauSanVuon.Dtos;

public class SeatingAreaUpsertDto : IRequestDto
{
    public string Name { get; set; }
    public string Color { get; set; }
    public List<Point> TakenUpPositions { get; set; }
    public List<SeatingUpsertDto> Seatings { get; set; }

    public void TransformValues()
    {
        Name = Name?.ToNullIfEmpty();
        Color = Color?.ToNullIfEmpty();
        TakenUpPositions ??= new List<Point>();

        Seatings ??= new List<SeatingUpsertDto>();
        foreach (SeatingUpsertDto dto in Seatings)
        {
            dto.TransformValues();
        }
    }
}