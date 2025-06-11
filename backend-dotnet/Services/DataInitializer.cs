using System.Security.Claims;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Bogus;
using QuanNhauSanVuon.Services.Entities;

namespace QuanNhauSanVuon.Services;

public sealed class DataInitializer
{
    private DatabaseContext _context;
    private UserManager<User> _userManager;
    private RoleManager<Role> _roleManager;

    public void InitializeData(IApplicationBuilder builder)
    {
        Randomizer.Seed = new Random(8675309);
        using IServiceScope serviceScope = builder.ApplicationServices.CreateScope();
        _context = serviceScope.ServiceProvider.GetService<DatabaseContext>();
        _context.Database.EnsureCreated();
        _userManager = serviceScope.ServiceProvider.GetService<UserManager<User>>();
        _roleManager = serviceScope.ServiceProvider.GetService<RoleManager<Role>>();
        using var transaction = _context.Database.BeginTransaction();
        InitializeRoles();
        InitializeUsers();
        InitializeRoleClaims();
        InitializeMenuCategoryAndMenuItems();

        _context.SaveChanges();
        transaction.Commit();
    }

    private void InitializeRoles()
    {
        if (_roleManager.Roles.Any())
        {
            return;
        }

        Console.WriteLine("Initializing roles");
        List<Role> roles = new List<Role>
        {
            new Role
            {
                Name = "Developer",
                DisplayName = "Nhà phát triển",
                PowerLevel = 50
            },
            new Role
            {
                Name = "Manager",
                DisplayName = "Quản lý",
                PowerLevel = 40
            },
            new Role
            {
                Name = "Accountant",
                DisplayName = "Kế toán",
                PowerLevel = 30
            },
            new Role
            {
                Name = "Staff",
                DisplayName = "Nhân viên",
                PowerLevel = 20
            },
        };

        foreach (Role role in roles)
        {
            IdentityResult result = _roleManager
                .CreateAsync(role)
                .GetAwaiter()
                .GetResult();
            if (!result.Succeeded)
            {
                string errorMessage = result.Errors.FirstOrDefault()?.Description;
                throw new InvalidOperationException(errorMessage);
            }
            _context.SaveChanges();
        }
    }

    private void InitializeRoleClaims()
    {
    }

    private void InitializeUsers()
    {
        if (_userManager.Users.Any())
        {
            return;
        }

        Console.WriteLine("Initializing users");
        Dictionary<User, (string Password, string RoleName)> users;
        users = new Dictionary<User, (string Password, string RoleName)>
        {
            {
                new User
                {
                    UserName = "ngokhanhhuyy"
                },
                ("Huyy47b1", "Developer")
            },
            {
                new User
                {
                    UserName = "admin",
                },
                (null, "Manager")
            },
            {
                new User
                {
                    UserName = "accountant",
                },
                (null, "Accountant")
            },
            {
                new User
                {
                    UserName = "staff",
                },
                (null, "Staff")
            }
        };
        foreach (KeyValuePair<User, (string Password, string RoleName)> pair in users)
        {
            IdentityResult result = _userManager
                .CreateAsync(pair.Key, pair.Value.Password ?? pair.Key.UserName)
                .GetAwaiter()
                .GetResult();
            string errorMessage;
            if (!result.Succeeded)
            {
                errorMessage = result.Errors.FirstOrDefault()?.Description;
                throw new InvalidOperationException(errorMessage);
            }

            result = _userManager
                .AddToRoleAsync(pair.Key, pair.Value.RoleName)
                .GetAwaiter()
                .GetResult();
            if (!result.Succeeded)
            {
                errorMessage = result.Errors.FirstOrDefault()?.Description;
                throw new InvalidOperationException(errorMessage);
            }
        }

        _context.SaveChanges();
    }

    private void InitializeMenuCategoryAndMenuItems()
    {
        if (_context.MenuItems.Any())
        {
            return;
        }

        Console.WriteLine("Initializing menu categories");
        List<MenuCategory> menuCategories = new List<MenuCategory>
        {
            new MenuCategory
            {
                Name = "Gà",
                Items = new List<MenuItem>
                {
                    new() { Name = "Gà Chiên Mắm", DefaultNetPrice = 89_000, Unit = "Phần" },
                    new() { Name = "Gà Chiên Bơ", DefaultNetPrice = 89_000, Unit = "Phần" },
                    new() { Name = "Gà Rang Muối", DefaultNetPrice = 89_000, Unit = "Phần" },
                    new() { Name = "Gà Nướng Muối Ớt", DefaultNetPrice = 89_000, Unit = "Phần" },
                    new() { Name = "Gà Nấu Lá É", DefaultNetPrice = 139_000, Unit = "Phần" },
                    new() { Name = "Gà Tiềm Ớt Hiểm", DefaultNetPrice = 139_000, Unit = "Phần" },
                    new() { Name = "Lẩu Gà Lá Giang", DefaultNetPrice = 139_000, Unit = "Phần" },
                }
            },
            new MenuCategory
            {
                Name = "Cơm - Mì",
                Items = new()
                {
                    new() { Name = "Mì Xào Rau Nấm", DefaultNetPrice = 40_000, Unit = "Phần" },
                    new() { Name = "Mì Xào Bò", DefaultNetPrice = 60_000, Unit = "Phần" },
                    new() { Name = "Mì Xào Hải Sản", DefaultNetPrice = 60_000, Unit = "Phần" },
                    new() { Name = "Cơm Chiên Trứng", DefaultNetPrice = 40_000, Unit = "Phần" },
                    new() { Name = "Cơm Chiên Tỏi", DefaultNetPrice = 40_000, Unit = "Phần" },
                    new()
                    {
                        Name = "Cơm Chiên Muối Ớt",
                        DefaultNetPrice = 40_000,
                        Unit = "Phần"
                    },
                    new()
                    {
                        Name = "Cơm Chiên Hải Sản",
                        DefaultNetPrice = 60_000,
                        Unit = "Phần"
                    },
                    new()
                    {
                        Name = "Cơm Chiên Dương Châu",
                        DefaultNetPrice = 60_000,
                        Unit = "Phần"
                    },
                    new() { Name = "Cơm Chiên Cá Mặn", DefaultNetPrice = 60_000, Unit = "Phần" }
                }
            },
            new MenuCategory
            {
                Name = "Cá",
                Items = new()
                {
                    new() { Name = "Cá Bóp Nướng", DefaultNetPrice = 139_000, Unit = "Con" },
                    new() { Name = "Cá Bóp Nấu Mẻ", DefaultNetPrice = 139_000, Unit = "Con" },
                    new() { Name = "Lẩu Cá Bóp", DefaultNetPrice = 139_000, Unit = "Con" }
                }
            },
            new MenuCategory
            {
                Name = "Ếch",
                Items = new()
                {
                    new() { Name = "Ếch Chiên Bơ", DefaultNetPrice = 75_000, Unit = "Phần" },
                    new() { Name = "Ếch Chiên Mắm", DefaultNetPrice = 75_000, Unit = "Phần" },
                    new() { Name = "Ếch Xào Sả Ớt", DefaultNetPrice = 75_000, Unit = "Phần" },
                    new() { Name = "Ếch Nướng Muối Ớt", DefaultNetPrice = 75_000, Unit = "Phần" }
                }
            },
            new MenuCategory
            {
                Name = "Tôm Mực",
                Items = new()
                {
                    new() { Name = "Tôm Hấp", DefaultNetPrice = 75_000, Unit = "Phần" },
                    new()
                    {
                        Name = "Tôm Nướng Mắm Nhĩ",
                        DefaultNetPrice = 75_000,
                        Unit = "Phần"
                    },
                    new() { Name = "Tôm Nướng Muối", DefaultNetPrice = 75_000, Unit = "Phần" },
                    new() { Name = "Mực Hấp", DefaultNetPrice = 75_000, Unit = "Phần" },
                    new() { Name = "Mực Nướng", DefaultNetPrice = 75_000, Unit = "Phần" }
                }
            },
            new MenuCategory
            {
                Name = "Đặc biệt",
                Items = new()
                {
                    new() { Name = "Gân Đuôi Bò Hấp", DefaultNetPrice = 99_000, Unit = "Phần" },
                    new() { Name = "Gân Bò Trộn Cóc", DefaultNetPrice = 89_000, Unit = "Phần" },
                    new() { Name = "Lòng Bò Nấu Đắng", DefaultNetPrice = 99_000, Unit = "Phần" }
                }
            },
            new MenuCategory
            {
                Name = "Đồ uống",
                Items = new()
                {
                    new() { Name = "Tiger Bạc", DefaultNetPrice = 330_000, Unit = "Thùng" },
                    new() { Name = "Tiger Nâu", DefaultNetPrice = 330_000, Unit = "Thùng" },
                    new() { Name = "Saigon Larue", DefaultNetPrice = 250_000, Unit = "Thùng" },
                    new() { Name = "Saigon Lager", DefaultNetPrice = 260_000, Unit = "Thùng" },
                    new() { Name = "Ken Lon Xanh", DefaultNetPrice = 360_000, Unit = "Thùng" },
                    new() { Name = "Ken Chai", DefaultNetPrice = 380_000, Unit = "Thùng" },
                    new() {
                        Name = "Saigon Đỏ Lon Cao",
                        DefaultNetPrice = 280_000,
                        Unit = "Thùng"
                    },
                    new() { Name = "Saigon Special", DefaultNetPrice = 310_000, Unit = "Thùng" },
                    new() { Name = "Saigon Chill", DefaultNetPrice = 310_000, Unit = "Thùng" },
                    new() { Name = "Pepsi", DefaultNetPrice = 180_000, Unit = "Thùng" },
                    new() { Name = "Coca", DefaultNetPrice = 180_000, Unit = "Thùng" },
                    new() { Name = "7Up", DefaultNetPrice = 180_000, Unit = "Thùng" },
                    new() { Name = "Sting", DefaultNetPrice = 180_000, Unit = "Thùng" },
                    new() { Name = "Nước Suối", DefaultNetPrice = 90_000, Unit = "Thùng" },
                    new() { Name = "Bò Húc Thái", DefaultNetPrice = 320_000, Unit = "Thùng" },
                    new() { Name = "Bò Húc Xanh", DefaultNetPrice = 300_000, Unit = "Thùng" }
                }
            },
        };

        _context.MenuCategories.AddRange(menuCategories);
        _context.SaveChanges();
    }
}