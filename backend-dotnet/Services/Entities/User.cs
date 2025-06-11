using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace QuanNhauSanVuon.Services.Entities;

[Table("users")]
public class User : IdentityUser<int>
{
    [Column("is_deleted")]
    [Required]
    public bool IsDeleted { get; set; }

    [Column("row_version")]
    [Timestamp]
    public byte[] RowVersion { get; set; }

    // Navigation properties.
    public virtual List<Role> Roles { get; set; }
    public virtual List<Order> CreatedOrders { get; set; }
    public virtual List<Order> LastUpdatedOrders { get; set; }

    // Computed properties.
    [NotMapped]
    public Role Role => Roles.SingleOrDefault();
}