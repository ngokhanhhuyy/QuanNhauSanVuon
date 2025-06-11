using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using QuanNhauSanVuon.Extensions;

namespace QuanNhauSanVuon.Services.Entities;

[Table("menu_items")]
public class MenuItem
{
    [Column("id")]
    [Key]
    public int Id { get; set; }

    [Column("name")]
    [Required]
    [StringLength(150)]
    public required string Name { get; set; }

    [Column("default_net_price")]
    [Required]
    public long DefaultNetPrice { get; set; }

    [Column("default_vat_percentage")]
    [Required]
    public int DefaultVatPercentage { get; set; }

    [Column("unit")]
    [Required]
    [StringLength(50)]
    public required string Unit { get; set; }

    [Column("thumbnail_url")]
    [StringLength(255)]
    public string ThumbnailUrl { get; set; }

    [Column("created_datetime")]
    public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow.ToApplicationTime();

    [Column("last_updated_datetime")]
    public DateTime? LastUpdatedDateTime { get; set; }

    [Column("is_deleted")]
    [Required]
    public bool IsDeleted { get; set; }

    // Foreign key.
    [Column("category_id")]
    public int? CategoryId { get; set; }

    // Navigation properties.
    public virtual MenuCategory Category { get; set; }
    public virtual List<OrderItem> OrderItems { get; set; }
}
