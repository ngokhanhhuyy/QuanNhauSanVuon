using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanNhauSanVuon.Services.Entities;

[Table("seatings")]
public class Seating
{
    [Column("id")]
    [Key]
    public int Id { get; set; }
    
    [Column("name")]
    [Required]
    [StringLength(50)]
    public required string Name { get; set; }
    
    [Column("position_x")]
    [Required]
    public required int PositionX { get; set; }
    
    [Column("position_y")]
    [Required]
    public required int PositionY { get; set; }

    // Foreign keys.
    [Column("area_id")]
    public int? AreaId { get; set; }

    // Navigation properties.
    public virtual List<Order> Orders { get; set; }
    public virtual SeatingArea Area { get; set; }
}