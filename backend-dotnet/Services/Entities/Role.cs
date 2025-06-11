using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace QuanNhauSanVuon.Services.Entities;

[Table("roles")]
public class Role : IdentityRole<int>
{
    [Column("display_name")]
    [Required]
    [StringLength(50)]
    public required string DisplayName { get; set; }
    
    [Column("power_level")]
    [Required]
    public int PowerLevel { get; set; }
    
    // Navigation properties.
    public virtual List<User> Users { get; set; }
    public virtual List<IdentityRoleClaim<int>> Claims { get; set; }
}