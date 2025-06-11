using QuanNhauSanVuon.Services.Entities;

namespace QuanNhauSanVuon.Dtos;

public class OrderListDto
{
    public int PageCount { get; set; }
    public List<OrderBasicDto> Items { get; set; }

    public OrderListDto()
    {
        Items = new List<OrderBasicDto>();
    }

    public OrderListDto(int pageCount, List<Order> items)
    {
        PageCount = pageCount;
        Items = items.Select(i => new OrderBasicDto(i)).ToList();
    }
}