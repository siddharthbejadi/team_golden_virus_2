using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using team_golden_virus.Services;
using team_golden_virus.Models;
using team_golden_virus.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add EF Core DbContext (SQL Server)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Identity using ApplicationUser and IdentityRole
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Add cookie authentication (Identity will configure its own cookies, but keep explicit scheme for compatibility)
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/Login";
    });

// Simple in-memory user service (keep for legacy code if used elsewhere)
builder.Services.AddSingleton<IUserService, InMemoryUserService>();

var app = builder.Build();

// Seed identity roles and users (development convenience)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        // Ensure database is created before using UserManager/RoleManager
        var db = services.GetRequiredService<ApplicationDbContext>();
        db.Database.EnsureCreated();

        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

        // Roles to ensure
        string[] roles = new[] { "Admin", "Clinician", "Patient" };
        foreach (var role in roles)
        {
            var exists = roleManager.RoleExistsAsync(role).GetAwaiter().GetResult();
            if (!exists)
                roleManager.CreateAsync(new IdentityRole(role)).GetAwaiter().GetResult();
        }

        // Seed admin
        var adminEmail = "siddarthbejadi@fakemail.com";
        var admin = userManager.FindByEmailAsync(adminEmail).GetAwaiter().GetResult();
        if (admin == null)
        {
            admin = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                FullName = "siddarth",
                UserRole = "Admin",
                EmailConfirmed = true
            };
            var createAdmin = userManager.CreateAsync(admin, "Admin@123").GetAwaiter().GetResult();
            if (createAdmin.Succeeded)
                userManager.AddToRoleAsync(admin, "Admin").GetAwaiter().GetResult();
        }

        // Seed clinicians
        var clinicians = new[] {
            new { Email = "manmeet12@fakemail.com", FullName = "Manmeeth Singh" },
            new { Email = "harneetkaur12@fakemail.com", FullName = "Harneet Kaur" }
        };
        foreach (var c in clinicians)
        {
            var u = userManager.FindByEmailAsync(c.Email).GetAwaiter().GetResult();
            if (u == null)
            {
                u = new ApplicationUser
                {
                    UserName = c.Email,
                    Email = c.Email,
                    FullName = c.FullName,
                    UserRole = "Clinician",
                    EmailConfirmed = true
                };
                var res = userManager.CreateAsync(u, "Clinician@123").GetAwaiter().GetResult();
                if (res.Succeeded)
                    userManager.AddToRoleAsync(u, "Clinician").GetAwaiter().GetResult();
            }
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Error seeding database");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
