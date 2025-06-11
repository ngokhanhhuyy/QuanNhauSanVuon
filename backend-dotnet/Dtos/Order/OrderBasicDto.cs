using QuanNhauSanVuon.Services.Entities;

namespace QuanNhauSanVuon.Dtos;

public class OrderBasicDto
{
    public int Id { get; set; }
    public long NetAmount { get; set; }
    public long VatAmount { get; set; }
    public DateTime CreatedDateTime { get; set; }
    public DateTime? PaidDateTime { get; set; }
    public UserDto CreatedUser { get; set; }

    public OrderBasicDto(Order order)
    {
        Id = order.Id;
        NetAmount = order.NetAmount;
        VatAmount = order.VatAmount;
        CreatedDateTime = order.CreatedDateTime;
        PaidDateTime = order.PaidDateTime;
        CreatedUser = new UserDto(order.CreatedUser);
    }
}