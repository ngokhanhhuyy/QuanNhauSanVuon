using QuanNhauSanVuon.Dtos.Interfaces;

namespace QuanNhauSanVuon.Dtos;

public class OrderItemUpsertDto : IRequestDto
{
    public int? Id { get; set; }
    public int Quantity { get; set; }
    public long? NetPricePerUnit { get; set; }
    public long? VatAmountPerUnit { get; set; }
    public int MenuItemId { get; set; }
    public bool HasBeenChanged { get; set; }
    public bool HasBeenDeleted { get; set; }

    public void TransformValues() { }
}