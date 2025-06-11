using QuanNhauSanVuon.Dtos.Interfaces;

namespace QuanNhauSanVuon.Dtos;

public class OrderUpsertDto : IRequestDto
{
    public int SeatingId { get; set; }
    public List<OrderItemUpsertDto> Items { get; set; }

    public void TransformValues()
    {
        Items ??= new List<OrderItemUpsertDto>();
    }
}