using QuanNhauSanVuon.Services.Entities;
using QuanNhauSanVuon.Services.Structs;

namespace QuanNhauSanVuon.Dtos;

public class SeatingAreaDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Color { get; set; }
    public List<PointDto> TakenUpPositions { get; set; }
    public List<SeatingMinimalDto> Seatings { get; set; }

    public SeatingAreaDto(SeatingArea seatingArea)
    {
        Id = seatingArea.Id;
        Name = seatingArea.Name;
        Color = seatingArea.Color;
        TakenUpPositions = seatingArea.TakenUpPositions
          .Select(point => new PointDto(point))
          .ToList();
        Seatings = seatingArea.Seatings.Select(s => new SeatingMinimalDto(s)).ToList();
    }
}