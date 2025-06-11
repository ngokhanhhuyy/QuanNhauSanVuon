using System.Text.Json;
using System.Globalization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using FluentValidation;
using QuanNhauSanVuon.Middlewares;
using QuanNhauSanVuon.Services;
using QuanNhauSanVuon.Services.Entities;
using QuanNhauSanVuon.Services.Identity;
using QuanNhauSanVuon.Validation;
using QuanNhauSanVuon.Validation.Validators;
using QuanNhauSanVuon.Services.Interfaces;
using QuanNhauSanVuon.Services.Exceptions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

// Connection string - EF Core.
string connectionString = builder.Configuration.GetConnectionString("Mysql");
builder.Services.AddDbContextFactory<DatabaseContext>(options => options
    .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
    .AddInterceptors(new VietnamTimeInterceptor()));

// Identity.
builder.Services.AddIdentity<User, Role>()
    .AddEntityFrameworkStores<DatabaseContext>()
    .AddErrorDescriber<VietnameseIdentityErrorDescriber>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 0;
});

// Cookie.
builder.Services.ConfigureApplicationCookie(options =>
{
    options.ExpireTimeSpan = TimeSpan.FromDays(30);
    options.SlidingExpiration = false;
    options.Cookie.Name = "QuanNhauSanVuonAuthenticationCookie";
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.LoginPath = "/SignIn";
    options.LogoutPath = "/Logout";
    
    options.Events.OnRedirectToLogin = options.Events.OnRedirectToAccessDenied = (context) =>
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        return Task.CompletedTask;
    };

    options.Events.OnRedirectToLogout = (context) =>
    {
        context.Response.StatusCode = StatusCodes.Status200OK;
        return Task.CompletedTask;
    };
});

// Authentication by cookie strategies.
builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme);

// FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<SignInValidator>();
ValidatorOptions.Global.LanguageManager.Enabled = true;
ValidatorOptions.Global.LanguageManager = new ValidatorLanguageManager
{
    Culture = new CultureInfo("vi")
};
ValidatorOptions.Global.PropertyNameResolver = (_, b, _) => b.Name
    .First()
    .ToString()
    .ToLower() + b.Name[1..];

// Add controllers with json serialization policy.
builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
    });

// Dependency injection.
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<SignInManager<User>>();
builder.Services.AddTransient<RoleManager<Role>>();
builder.Services.AddTransient<DatabaseContext>();
builder.Services.AddSingleton<MySqlExceptionHandler>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IAuthorizationService, AuthorizationService>();
builder.Services.AddScoped<ISeatingAreaService, SeatingAreaService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IMenuItemService, MenuItemService>();
builder.Services.AddScoped<IMenuCategoryService, MenuCategoryService>();

// Api explorer.
builder.Services.AddEndpointsApiExplorer();

// CORS.
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: "LocalhostDevelopment",
        policyBuilder => policyBuilder
            .AllowAnyHeader()
            .AllowAnyMethod());
});

WebApplication app = builder.Build();
DataInitializer dataInitializer;
dataInitializer = new DataInitializer();
dataInitializer.InitializeData(app);

app.UseCors("LocalhostDevelopment");
app.Use(async (context, next) =>
{
    if (context.Request.Headers.TryGetValue("Origin", out StringValues originHeader))
    {
        string origin = originHeader.ToString();
        context.Response.Headers.AccessControlAllowOrigin = origin;
        context.Response.Headers.AccessControlAllowCredentials = "true";
    }

    await next();
});

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseMiddleware<ExceptionHandlerMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseRouting();
app.UseResponseCaching();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
// app.UseStaticFiles();
app.Run();