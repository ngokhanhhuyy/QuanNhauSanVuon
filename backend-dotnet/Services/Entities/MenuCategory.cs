using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanNhauSanVuon.Services.Entities;

[Table("menu_categories")]
public class MenuCategory
{
    [Column("id")]
    [Key]
    public int Id { get; set; }

    [Column("name")]
    [Required]
    [StringLength(50)]
    public required string Name { get; set; }

    // Navigation properties.
    public virtual List<MenuItem> Items { get; set; }
}