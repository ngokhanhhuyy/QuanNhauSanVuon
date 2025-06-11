using QuanNhauSanVuon.Dtos.Interfaces;

namespace QuanNhauSanVuon.Dtos;

public class MonthYearDto : IRequestDto
{
    public int Month { get; set; }
    public int Year { get; set; }

    public void TransformValues() { }
}