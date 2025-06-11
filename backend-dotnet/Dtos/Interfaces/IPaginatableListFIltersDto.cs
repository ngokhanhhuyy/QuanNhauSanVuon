using QuanNhauSanVuon.Dtos.Interfaces;

namespace QuanNhauSanVuon.Dtos;

public interface IPaginatableListFiltersDto : IRequestDto
{
    int Page { get; set; }
    int ResultsPerPage { get; set; }
}