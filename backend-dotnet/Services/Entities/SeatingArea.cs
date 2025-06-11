using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using QuanNhauSanVuon.Services.Structs;

namespace QuanNhauSanVuon.Services.Entities;

[Table("seating_areas")]
public class SeatingArea
{
    [Column("id")]
    [Key]
    public int Id { get; set; }

    [Column("name")]
    [StringLength(50)]
    public string Name { get; set; }

    [Column("color")]
    [StringLength(30)]
    public string Color { get; set; }

    [Column("taken_up_positions_json")]
    [Required]
    [StringLength(5000)]
    public string TakenUpPositionsJson { get; set; }

    // Navigation properties.
    public virtual List<Seating> Seatings { get; set; }

    // Computed properties.
    [NotMapped]
    public List<Point> TakenUpPositions
    {
        get => JsonSerializer.Deserialize<List<Point>>(TakenUpPositionsJson);
        set => TakenUpPositionsJson = JsonSerializer.Serialize(value);
    }
}