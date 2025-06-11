using QuanNhauSanVuon.Services.Entities;

namespace QuanNhauSanVuon.Dtos;

public class OrderDetailDto : OrderBasicDto
{
    public DateTime? LastUpdatedDateTime { get; set; }
    public UserDto LastUpdatedUser { get; set; }
    public List<OrderItemDto> Items { get; set; }

    public OrderDetailDto(Order order) : base(order)
    {
        LastUpdatedDateTime = order.LastUpdatedDateTime;
        Items = order.Items.Select(item => new OrderItemDto(item)).ToList();

        if (order.LastUpdatedUser != null)
        {
            LastUpdatedUser = new UserDto(order.LastUpdatedUser);
        }
    }
}