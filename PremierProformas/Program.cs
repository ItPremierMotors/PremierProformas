using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PremierProformas.Data;

var builder = WebApplication.CreateBuilder(args);

var adminEmail = builder.Configuration["Admin:Email"];
var adminPassword = builder.Configuration["Admin:Password"];
if (string.IsNullOrWhiteSpace(adminEmail) || string.IsNullOrWhiteSpace(adminPassword))
{
    throw new InvalidOperationException("Configure Admin:Email y Admin:Password antes de iniciar la aplicación.");
}

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString).UseSnakeCaseNamingConvention());
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Lockout.AllowedForNewUsers = true;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
    options.Lockout.MaxFailedAccessAttempts = 5;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Aplica las migraciones pendientes al arrancar (crea las tablas en PostgreSQL si no existen).
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await db.Database.MigrateAsync();

    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    foreach (var roleName in new[] { "Admin", "JefeVentas", "Vendedor" })
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            var roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
            if (!roleResult.Succeeded)
            {
                throw new InvalidOperationException($"No se pudo crear el rol {roleName}: {string.Join(", ", roleResult.Errors.Select(error => error.Description))}");
            }
        }
    }

    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
    if (await userManager.FindByEmailAsync(adminEmail) is null)
    {
        var adminUser = new IdentityUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true,
            LockoutEnabled = true
        };
        var createResult = await userManager.CreateAsync(adminUser, adminPassword);
        if (!createResult.Succeeded)
        {
            throw new InvalidOperationException($"No se pudo crear el usuario administrador: {string.Join(", ", createResult.Errors.Select(error => error.Description))}");
        }

        var addRoleResult = await userManager.AddToRoleAsync(adminUser, "Admin");
        if (!addRoleResult.Succeeded)
        {
            throw new InvalidOperationException($"No se pudo asignar el rol Admin: {string.Join(", ", addRoleResult.Errors.Select(error => error.Description))}");
        }
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();

app.Run();
