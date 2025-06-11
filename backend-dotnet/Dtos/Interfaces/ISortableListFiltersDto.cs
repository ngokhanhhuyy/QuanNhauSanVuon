namespace QuanNhauSanVuon.Dtos.Interfaces;

public interface ISortableListFiltersDto : IRequestDto
{
    public bool SortedByAscending { get; set; }
    public string SortedByField { get; set; }
}