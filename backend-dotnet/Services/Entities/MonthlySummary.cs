using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using QuanNhauSanVuon.Extensions;

namespace QuanNhauSanVuon.Services.Entities;

[Table("monthly_summaries")]
public class MonthlySummary
{
    [Column("id")]
    [Key]
    public int Id { get; set; }

    [Column("year")]
    [Required]
    public int Year { get; set; } = DateTime.UtcNow.ToApplicationTime().Year;

    [Column("month")]
    [Required]
    public int Month { get; set; } = DateTime.UtcNow.ToApplicationTime().Month;

    [Column("revenue")]
    [Required]
    public long Revenue { get; set; }

    [Column("collected_vat_amount")]
    [Required]
    public long CollectedVatAmount { get; set; }
}