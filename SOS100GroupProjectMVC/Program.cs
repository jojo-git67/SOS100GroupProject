using Microsoft.EntityFrameworkCore;
using SOS100GroupProjectMVC.Data;
using SOS100GroupProjectMVC.Models;

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

        var app = builder.Build();
        
        // Apply database migration at startup
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<UserDbContext>();
            dbContext.Database.Migrate();


            if (!dbContext.Users.Any(u => u.UserName == "admin"))
            {
                var adminCredentials = new UserCredentials
                {
                    UserName = "admin",
                    Salt = "AdminSalt123",
                    Password = "b9b298342c781ecbc6484a2f394f8f0a8e7c94617e40b01f2126fab957b321f6"
                };
                dbContext.UserCredentials.Add(adminCredentials);
                dbContext.SaveChanges();
                
                var adminUser = new User
                {
                    UserName = "admin",
                    UserEmail = "admin@example.com",
                    FirstName = "Admin",
                    LastName = "User",
                    Role = "IT-admin"
                };
                dbContext.Users.Add(adminUser);
                dbContext.SaveChanges();
            }
            
        }

        // Configure the HTTP request pipeline.
        //if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        
        app.MapStaticAssets();
        app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Login}/{action=Index}/{id?}")
            .WithStaticAssets();

        app.Run();
    }
}