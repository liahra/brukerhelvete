using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;
using Noested.Data;
using Noested.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString,
    new MySqlServerVersion(new Version(10, 5, 11))));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IServiceOrderRepository, ServiceOrderRepository>(); // Daniel
builder.Services.AddScoped<ServiceOrderService>(); // Daniel
builder.Services.AddScoped<ChecklistService>(); // Daniel
builder.Services.AddScoped<CustomerService>(); // Daniel

var app = builder.Build();



// Database seeding
using (var serviceScope = app.Services.CreateScope())
{
    var services = serviceScope.ServiceProvider;

    // Hentrt UserManager og RoleManager tjenestene
    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

    // Sjekk for at nødvendige roller eksisterer
    var roles = new List<string> { "Admin", "Servicedesk", "Verksted" }; // Legger til nødvendige roller
    foreach (var roleName in roles)
    {
        if (!roleManager.RoleExistsAsync(roleName).Result)
        {
            var roleResult = roleManager.CreateAsync(new IdentityRole(roleName)).Result;
        }
    }

    // Definerer brukerinformasjon
    var users = new List<IdentityUser>
    {
        new IdentityUser { UserName = "user1@example.com", Email = "user1@example.com" },
        new IdentityUser { UserName = "user2@example.com", Email = "user2@example.com" }
    };

    // Oppretter brukere og tilordne roller
    foreach (var user in users)
    {
        if (userManager.FindByNameAsync(user.UserName).Result == null)
        {
            var result = userManager.CreateAsync(user, "YourPassword123!").Result;
            if (result.Succeeded)
            {
                // Tilordne roller. Setter "User" rollen til alle brukere.
                var roleResult = userManager.AddToRoleAsync(user, "-").Result;
            }
        }
    }

    // Fortsetter med eventuell annen seeding
    var serviceOrderDatabase = services.GetRequiredService<IServiceOrderRepository>();
    await DatabaseSeeder.SeedServiceOrders(serviceOrderDatabase);
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

// Tilpasset Middleware for Sikkerhetsheadere
app.Use(async (context, next) =>
{
    context.Response.Headers.Add("X-Xss-Protection", "1");
    context.Response.Headers.Add("X-Frame-Options", "DENY");
    context.Response.Headers.Add("Referrer-Policy", "no-referrer");
    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Add(
        "Content-Security-Policy",
        "default-src 'self'; " +
        "img-src 'self'; " +
        "font-src 'self'; " +
        "style-src 'self' 'unsafe-inline' https://stackpath.bootstrapcdn.com; " +
        "script-src 'self'; " +
        "frame-src 'self'; " +
        "connect-src 'self';");
    await next();
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");
app.MapFallbackToAreaPage("/Account/Login", "Identity");

app.Run();