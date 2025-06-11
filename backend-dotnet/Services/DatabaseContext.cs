using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using QuanNhauSanVuon.Services.Entities;

namespace QuanNhauSanVuon.Services;

public class DatabaseContext
    : IdentityDbContext<User, Role, int, IdentityUserClaim<int>, IdentityUserRole<int>,
        IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

    public DbSet<SeatingArea> SeatingAreas { get; set; }
    public DbSet<Seating> Seatings { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<MenuItem> MenuItems { get; set; }
    public DbSet<MenuCategory> MenuCategories { get; set; }
    public DbSet<MonthlySummary> MonthlySummaries { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<SeatingArea>(e =>
        {
            e.HasKey(sa => sa.Id);
            e.HasIndex(sa => sa.Name)
                .IsUnique()
                .HasDatabaseName("unique__seating_areas__name");
        });
        builder.Entity<Seating>(e =>
        {
            e.HasKey(s => s.Id);
            e.HasOne(s => s.Area)
                .WithMany(sa => sa.Seatings)
                .HasForeignKey(s => s.AreaId)
                .HasConstraintName("fk__seatings__seating_areas__area_id")
                .OnDelete(DeleteBehavior.Restrict);
            e.HasIndex(s => s.Name).IsUnique().HasDatabaseName("unique__seatings__name");
        });

        builder.Entity<Order>(e =>
        {
            e.HasKey(o => o.Id);
            e.HasOne(o => o.Seating)
                .WithMany(s => s.Orders)
                .HasForeignKey(o => o.SeatingId)
                .HasConstraintName("fk__orders__seatings__seating_id")
                .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(o => o.CreatedUser)
                .WithMany(u => u.CreatedOrders)
                .HasForeignKey(o => o.CreatedUserId)
                .HasConstraintName("fk__orders__users__created_user_id")
                .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(o => o.LastUpdatedUser)
                .WithMany(u => u.LastUpdatedOrders)
                .HasForeignKey(o => o.LastUpdatedUserId)
                .HasConstraintName("fk__orders__users__last_updated_user_id")
                .OnDelete(DeleteBehavior.SetNull);
            e.Property(o => o.RowVersion).IsRowVersion();
        });

        builder.Entity<OrderItem>(e =>
        {
            e.HasKey(oi => oi.Id);
            e.HasOne(oi => oi.Order)
                .WithMany(o => o.Items)
                .HasForeignKey(oi => oi.OrderId)
                .HasConstraintName("fk__order_items__orders__order_id")
                .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(oi => oi.MenuItem)
                .WithMany(mi => mi.OrderItems)
                .HasForeignKey(oi => oi.MenuItemId)
                .HasConstraintName("fk__order_items__menu_items__menu_item_id")
                .OnDelete(DeleteBehavior.Restrict);
            e.Property(o => o.RowVersion).IsRowVersion();
        });

        builder.Entity<MenuItem>(e =>
        {
            e.HasKey(mi => mi.Id);
            e.HasOne(mi => mi.Category)
                .WithMany(mc => mc.Items)
                .HasForeignKey(mi => mi.CategoryId)
                .HasConstraintName("fk__menu_items__menu_categories__category_id")
                .OnDelete(DeleteBehavior.SetNull);
        });

        builder.Entity<MenuCategory>(e =>
        {
            e.HasKey(mc => mc.Id);
            e.HasIndex(mc => mc.Name)
                .IsUnique()
                .HasDatabaseName("unique__menu_categories__name");
        });

        // Identity entities
        builder.Entity<User>(e =>
        {
            e.ToTable("users");
            e.HasKey(u => u.Id);
            e.Property(u => u.Id).HasColumnName("id");
            e.Property(u => u.UserName).HasColumnName("username");
            e.Property(u => u.AccessFailedCount).HasColumnName("access_failed_count");
            e.Property(u => u.ConcurrencyStamp).HasColumnName("concurrent_stamp");
            e.Property(u => u.Email).HasColumnName("email");
            e.Property(u => u.EmailConfirmed).HasColumnName("email_confirmed");
            e.Property(u => u.LockoutEnabled).HasColumnName("lockout_enabled");
            e.Property(u => u.LockoutEnd).HasColumnName("lockout_end");
            e.Property(u => u.NormalizedEmail).HasColumnName("normalized_email");
            e.Property(u => u.NormalizedUserName).HasColumnName("normalized_username");
            e.Property(u => u.PasswordHash).HasColumnName("password_hash");
            e.Property(u => u.PhoneNumber).HasColumnName("phone_number");
            e.Property(u => u.PhoneNumberConfirmed).HasColumnName("phone_number_confirmed");
            e.Property(u => u.SecurityStamp).HasColumnName("security_stamp");
            e.Property(u => u.TwoFactorEnabled).HasColumnName("two_factor_enabled");
            e.Property(u => u.SecurityStamp).HasColumnName("security_stamp");
            e.HasMany(u => u.Roles)
                .WithMany(r => r.Users)
                .UsingEntity<IdentityUserRole<int>>(ur => ur.ToTable("user_roles"));
            e.HasIndex(u => u.UserName)
                .IsUnique()
                .HasDatabaseName("unique__users__username");
            e.Property(o => o.RowVersion).IsRowVersion();
        });
        builder.Entity<Role>(e =>
        {
            e.ToTable("roles");
            e.HasKey(r => r.Id);
            e.Property(r => r.Id).HasColumnName("id");
            e.Property(r => r.Name).HasColumnName("name");
            e.Property(r => r.NormalizedName).HasColumnName("normalized_name");
            e.Property(r => r.ConcurrencyStamp).HasColumnName("concurrent_stamp");
            e.HasIndex(r => r.Name)
                .IsUnique()
                .HasDatabaseName("unique__roles__name");
            e.HasIndex(r => r.DisplayName)
                .IsUnique()
                .HasDatabaseName("unique__roles__display_name");
        });
        builder.Entity<IdentityUserRole<int>>(e =>
        {
            e.ToTable("user_roles");
            e.Property(ur => ur.UserId).HasColumnName("user_id");
            e.Property(ur => ur.RoleId).HasColumnName("role_id");
        });
        builder.Entity<IdentityUserClaim<int>>(e =>
        {
            e.ToTable("user_claims");
            e.Property(uc => uc.Id).HasColumnName("id");
            e.Property(uc => uc.UserId).HasColumnName("user_id");
            e.Property(uc => uc.ClaimType).HasColumnName("claim_type");
            e.Property(uc => uc.ClaimValue).HasColumnName("claim_value");
        });
        builder.Entity<IdentityUserLogin<int>>(e =>
        {
            e.ToTable("user_logins");
            e.HasKey(ul => ul.UserId);
            e.Property(ul => ul.UserId).HasColumnName("user_id");
            e.Property(ul => ul.LoginProvider).HasColumnName("login_providers");
            e.Property(ul => ul.ProviderDisplayName).HasColumnName("provider_display_name");
            e.Property(ul => ul.ProviderKey).HasColumnName("provider_key");
        });
        builder.Entity<IdentityUserToken<int>>(e =>
        {
            e.ToTable("user_tokens");
            e.HasKey(ut => ut.UserId);
        });
        builder.Entity<IdentityRoleClaim<int>>(e =>
        {
            e.ToTable("role_claims");
            e.Property(rc => rc.Id).HasColumnName("id");
            e.Property(rc => rc.ClaimType).HasColumnName("claim_type");
            e.Property(rc => rc.ClaimValue).HasColumnName("claim_value");
            e.Property(rc => rc.RoleId).HasColumnName("role_id");
        });
    }

    public string GetTableName<TEntity>() where TEntity : class
    {
        return Model.FindEntityType(typeof(TEntity))?.GetTableName();
    }

    public string GetColumnName<TEntity>(string propertyName) where TEntity : class
    {
        IEntityType entityType = Model.FindEntityType(typeof(TEntity));
        if (entityType == null)
        {
            return null;
        }

        IProperty property = entityType.FindProperty(propertyName);
        if (property == null)
        {
            return null;
        }

        return property.GetColumnName();
    }
}