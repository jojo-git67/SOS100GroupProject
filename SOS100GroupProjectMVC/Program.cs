using Microsoft.EntityFrameworkCore;
using SOS100GroupProjectMVC.Data;

namespace SOS100GroupProjectMVC;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();
        builder.Services.AddHttpClient();
        
        builder.Services.AddDbContext<UserDbContext>(options =>
            options.UseSqlite(
                builder.Configuration.GetConnectionString("DefaultConnection")));

        // Registrera UserDbContext
        builder.Services.AddDbContext<UserDbContext>(options =>
            options.UseSqlite(
                builder.Configuration.GetConnectionString("DefaultConnection")));

        var app = builder.Build();
        
        // Apply database migration at startup

        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var dbContext = services.GetRequiredService<UserDbContext>();
            dbContext.Database.Migrate();
            
        }

        // Configure the HTTP request pipeline.
        //if (!app.Environment.IsDevelopment())
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
                pattern: "{controller=Login}/{action=Index}/{id?}")
            .WithStaticAssets();

        app.Run();
    }
}