using System.Text.Json.Serialization;

namespace QuanNhauSanVuon.Services.Structs;

public struct Point
{
    public int X { get; set; }
    public int Y { get; set; }

    [JsonIgnore]
    public Point Top => new Point { X = X, Y = Y - 1 };

    [JsonIgnore]
    public Point Bottom => new Point { X = X, Y = Y + 1 };

    [JsonIgnore]
    public Point Left => new Point { X = X - 1, Y = Y };

    [JsonIgnore]
    public Point Right => new Point { X = X + 1, Y = Y };
    
    public override readonly bool Equals(object obj)
    {
        return obj is Point p && X == p.X && Y == p.Y;
    }

    public override readonly int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }
}