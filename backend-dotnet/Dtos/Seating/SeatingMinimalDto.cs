using QuanNhauSanVuon.Services.Entities;

namespace QuanNhauSanVuon.Dtos;

public class SeatingMinimalDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public PointDto Position { get; set; }

    public SeatingMinimalDto(Seating seating)
    {
        Id = seating.Id;
        Name = seating.Name;
        Position = new PointDto(seating.PositionX, seating.PositionY);
    }
}