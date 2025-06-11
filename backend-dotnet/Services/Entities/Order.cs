using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using QuanNhauSanVuon.Extensions;

namespace QuanNhauSanVuon.Services.Entities;

[Table("orders")]
public class Order
{
    [Column("id")]
    [Key]
    public int Id { get; set; }
    
    [Column("cached_net_amount")]
    [Required]
    public long CachedNetAmount { get; set; }
    
    [Column("cached_vat_amount")]
    [Required]
    public long CachedVatAmount { get; set; }

    [Column("cached_gross_amount")]
    [Required]
    public long CachedGrossAmount { get; set; }
    
    [Column("created_datetime")]
    [Required]
    public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow.ToApplicationTime();
    
    [Column("last_updated_datetime")]
    public DateTime? LastUpdatedDateTime { get; set; }
    
    [Column("paid_datetime")]
    public DateTime? PaidDateTime { get; set; }
    
    // Foreign keys.
    [Column("seating_id")]
    [Required]
    public required int SeatingId { get; set; }

    [Column("created_user_id")]
    [Required]
    public required int CreatedUserId { get; set; }

    [Column("last_updated_user_id")]
    public int? LastUpdatedUserId { get; set; }
    
    [Column("row_version")]
    [Timestamp]
    public byte[] RowVersion { get; set; }
    
    // Navigation properties.
    public virtual Seating Seating { get; set; }
    public virtual List<OrderItem> Items { get; set; }
    public virtual User CreatedUser { get; set; }
    public virtual User LastUpdatedUser { get; set; }

    // Computed properties.
    [NotMapped]
    public long NetAmount => Items.Sum(i => i.NetPricePerUnit * i.Quantity);

    [NotMapped]
    public long VatAmount => Items.Sum(i => i.VatAmountPerUnit * i.Quantity);

    [NotMapped]
    public long GrossAmount => Items.Sum(i => i.Amount);
}