using QuanNhauSanVuon.Dtos.Interfaces;
using QuanNhauSanVuon.Services.Enums;

namespace QuanNhauSanVuon.Dtos;

public class MenuItemListFiltersDto : ISortableListFiltersDto, IPaginatableListFiltersDto
{
    public bool SortedByAscending { get; set; } = true;
    public string SortedByField { get; set; } = nameof(SortedByFieldOption.Name);
    public int Page { get; set; } = 1;
    public int ResultsPerPage { get; set; } = 15;
    public int? CategoryId { get; set; }

    public void TransformValues()
    {
        CategoryId = CategoryId == 0 ? null : CategoryId;
    }
}