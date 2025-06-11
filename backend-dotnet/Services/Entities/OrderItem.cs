using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using QuanNhauSanVuon.Extensions;

namespace QuanNhauSanVuon.Services.Entities;

[Table("order_items")]
public class OrderItem
{
    [Column("id")]
    [Key]
    public int Id { get; set; }

    [Column("quantity")]
    [Required]
    public required int Quantity { get; set; }

    [Column("net_price_per_unit")]
    [Required]
    public required long NetPricePerUnit { get; set; }

    [Column("vat_amount_per_unit")]
    [Required]
    public required long VatAmountPerUnit { get; set; }

    [Column("ordered_datetime")]
    [Required]
    public DateTime OrderedDateTime { get; set; } = DateTime.UtcNow.ToApplicationTime();

    // Foreign keys.
    [Column("order_id")]
    [Required]
    public int OrderId { get; set; }

    [Column("menu_item_id")]
    [Required]
    public int MenuItemId { get; set; }
    
    [Column("row_version")]
    [Timestamp]
    public byte[] RowVersion { get; set; }
    
    // Navigation property.
    public virtual Order Order { get; set; }
    public virtual MenuItem MenuItem { get; set; }

    // Computed properties.
    [NotMapped]
    public long AmountPerUnit => NetPricePerUnit + VatAmountPerUnit;

    [NotMapped]
    public long Amount => AmountPerUnit * Quantity;
}
