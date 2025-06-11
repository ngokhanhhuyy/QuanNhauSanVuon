
using QuanNhauSanVuon.Dtos.Interfaces;
using QuanNhauSanVuon.Extensions;
using QuanNhauSanVuon.Services.Enums;

namespace QuanNhauSanVuon.Dtos;

public class OrderListFiltersDto : ISortableListFiltersDto, IPaginatableListFiltersDto
{
    public bool SortedByAscending { get; set; } = false;
    public string SortedByField { get; set; } = nameof(SortedByFieldOption.CreatedDateTime);
    public int Page { get; set; } = 1;
    public int ResultsPerPage { get; set; } = 15;
    public int? SeatingId { get; set; }
    public MonthYearDto CreatedMonthYear { get; set; }
    public MonthYearDto PaidMonthYear { get; set; }

    public void TransformValues()
    {
        SortedByField = SortedByField.ToNullIfEmpty();
    }
}