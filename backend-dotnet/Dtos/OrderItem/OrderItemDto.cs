using QuanNhauSanVuon.Services.Entities;

namespace QuanNhauSanVuon.Dtos;

public class OrderItemDto
{
    public int Id { get; set; }
    public int Quantity { get; set; }
    public long NetPricePerUnit { get; set; }
    public long VatAmountPerUnit { get; set; }
    public DateTime OrderedDateTime { get; set; }
    public MenuItemBasicDto MenuItem { get; set; }

    public OrderItemDto(OrderItem orderItem)
    {
        Id = orderItem.Id;
        Quantity = orderItem.Quantity;
        NetPricePerUnit = orderItem.NetPricePerUnit;
        VatAmountPerUnit = orderItem.VatAmountPerUnit;
        OrderedDateTime = orderItem.OrderedDateTime;
        MenuItem = new MenuItemBasicDto(orderItem.MenuItem);
    }
}