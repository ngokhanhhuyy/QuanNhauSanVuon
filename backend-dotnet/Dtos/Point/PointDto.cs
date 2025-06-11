using QuanNhauSanVuon.Dtos.Interfaces;
using QuanNhauSanVuon.Services.Structs;

namespace QuanNhauSanVuon.Dtos;

public class PointDto
{
    public int X { get; set; }
    public int Y { get; set; }

    public PointDto(int x, int y)
    {
        X = x;
        Y = y;
    }

    public PointDto(Point point)
    {
        X = point.X;
        Y = point.Y;
    }

    public void TransformValues() { }
}