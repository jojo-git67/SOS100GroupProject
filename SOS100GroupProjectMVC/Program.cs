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

            AddDefaultUsers(dbContext);
        }

        // Configure the HTTP request pipeline.
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

    private static void AddDefaultUsers(UserDbContext dbContext)
{
    var defaultUsers = new[]
    {
        new
        {
            UserName = "admin",
            Salt = "AdminSalt123",
            //password = admin!
            Password = "b9b298342c781ecbc6484a2f394f8f0a8e7c94617e40b01f2126fab957b321f6",
            Email = "admin@example.com",
            FirstName = "Admin",
            LastName = "User",
            Role = "IT-admin"
        },
        new
        {
            UserName = "teacher",
            Salt = "TeacherSalt123",
            //password = teacher!
            Password = "c093716ee0d51f0d509cd5e9d9207b09e86da8d9ddb8ba5ea8409fc9e0e81e0a",
            Email = "teacher@example.com",
            FirstName = "Teacher",
            LastName = "User",
            Role = "teacher"
        },
        new
        {
            UserName = "courseadmin",
            Salt = "CourseAdminSalt123",
            //password = courseadmin!
            Password = "4d414b3080e6a3505496c54b7cb49434472dd867a99b684772ffedcf945b4a45",
            Email = "courseadmin@example.com",
            FirstName = "Course",
            LastName = "Admin",
            Role = "courseAdmin"
        },
        new
        {
            UserName = "user",
            Salt = "UserSalt123",
            //password = student!
            Password = "f4f1ae6b632f48cab7d41e3e2ed3b4ff9adda0e49070aee2e05c75c257b4a339",
            Email = "user@example.com",
            FirstName = "Normal",
            LastName = "User",
            Role = "student"
        }
    };

    foreach (var u in defaultUsers)
    {
        if (!dbContext.Users.Any(x => x.UserName == u.UserName))
        {
            dbContext.UserCredentials.Add(new UserCredentials
            {
                UserName = u.UserName,
                Salt = u.Salt,
                Password = u.Password
            });

            dbContext.Users.Add(new User
            {
                UserName = u.UserName,
                UserEmail = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Role = u.Role
            });

            dbContext.SaveChanges();
        }
    }
}
}
