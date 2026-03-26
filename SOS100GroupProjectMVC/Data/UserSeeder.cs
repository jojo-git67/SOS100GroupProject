using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using SOS100GroupProjectMVC.Models;

namespace SOS100GroupProjectMVC.Data;

public static class UserSeeder
{
    public static void AddDefaultUsers(UserDbContext dbContext)
    {
        var defaultUsers = new[]
        {
            // IT-admin
            new { UserName = "admin", Salt = "AdminSalt123", Password = "admin!", Email = "admin@example.com", FirstName = "Admin", LastName = "User", Role = "IT-admin" },
            new { UserName = "admin2", Salt = "AdminSalt456", Password = "admin!", Email = "admin2@example.com", FirstName = "Admin2", LastName = "User", Role = "IT-admin" },

            // teacher
            new { UserName = "teacher", Salt = "TeacherSalt123", Password = "teacher!", Email = "teacher@example.com", FirstName = "Teacher", LastName = "User", Role = "teacher" },
            new { UserName = "teacher2", Salt = "TeacherSalt456", Password = "teacher!", Email = "teacher2@example.com", FirstName = "Teacher2", LastName = "User", Role = "teacher" },

            // courseAdmin
            new { UserName = "courseadmin", Salt = "CourseAdminSalt123", Password = "courseadmin!", Email = "courseadmin@example.com", FirstName = "Course", LastName = "Admin", Role = "courseAdmin" },
            new { UserName = "courseadmin2", Salt = "CourseAdminSalt456", Password = "courseadmin!", Email = "courseadmin2@example.com", FirstName = "Course2", LastName = "Admin", Role = "courseAdmin" },

            // student
            new { UserName = "user", Salt = "UserSalt123", Password = "student!", Email = "user@example.com", FirstName = "Normal", LastName = "User", Role = "student" },
            new { UserName = "user2", Salt = "UserSalt456", Password = "student!", Email = "user2@example.com", FirstName = "Normal2", LastName = "User", Role = "student" }
        };

        foreach (var u in defaultUsers)
        {
            var passwordHash = GetHashFunction(u.Salt + u.Password);

            // Upsert UserCredentials (primary key: UserName)
            var existingCredentials = dbContext.UserCredentials
                .SingleOrDefault(x => x.UserName == u.UserName);

            if (existingCredentials == null)
            {
                dbContext.UserCredentials.Add(new UserCredentials
                {
                    UserName = u.UserName,
                    Salt = u.Salt,
                    Password = passwordHash
                });
            }
            else
            {
                existingCredentials.Salt = u.Salt;
                existingCredentials.Password = passwordHash;
            }

            // Upsert User (unique index: UserName)
            var existingUser = dbContext.Users
                .SingleOrDefault(x => x.UserName == u.UserName);

            if (existingUser == null)
            {
                dbContext.Users.Add(new User
                {
                    UserName = u.UserName,
                    UserEmail = u.Email,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Role = u.Role
                });
            }
            else
            {
                existingUser.UserEmail = u.Email;
                existingUser.FirstName = u.FirstName;
                existingUser.LastName = u.LastName;
                existingUser.Role = u.Role;
            }
        }

        dbContext.SaveChanges();
    }

    // Must match the hashing logic used in LoginController.
    private static string GetHashFunction(string input)
    {
        using (SHA256 sha256Hash = SHA256.Create())
        {
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder builder = new StringBuilder();
            foreach (byte b in bytes)
            {
                builder.Append(b.ToString("x2"));
            }
            return builder.ToString();
        }
    }
}

