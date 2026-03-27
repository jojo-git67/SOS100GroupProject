using Microsoft.EntityFrameworkCore;
using SOS100GroupProjectMVC.Data;

namespace SOS100GroupProjectMVC;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddHttpClient(); 

        // Add services to the container.
        builder.Services.AddControllersWithViews(options =>
        {
            options.Filters.Add<SOS100GroupProjectMVC.Filters.AuthFilter>();
        });

        // Registrera UserDbContext
        builder.Services.AddDbContext<UserDbContext>(options =>
            options.UseSqlite(
                builder.Configuration.GetConnectionString("DefaultConnection")));

        var app = builder.Build();

        // Ensure DB schema exists and seed default users.
        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<UserDbContext>();
            db.Database.Migrate();
            UserSeeder.AddDefaultUsers(db);
        }

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthorization();

        app.MapStaticAssets();
        app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
            .WithStaticAssets();

        app.Run();
    }
}