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
            // IT-admin (2 st)
            new { UserName = "admin", Salt = "AdminSalt123", Password = "admin!", Email = "admin@example.com", FirstName = "Admin", LastName = "User", Role = "IT-admin" },
            new { UserName = "admin2", Salt = "AdminSalt456", Password = "admin!", Email = "admin2@example.com", FirstName = "Admin2", LastName = "User", Role = "IT-admin" },

            // teacher (2 st)
            new { UserName = "teacher", Salt = "TeacherSalt123", Password = "teacher!", Email = "teacher@example.com", FirstName = "Teacher", LastName = "User", Role = "teacher" },
            new { UserName = "teacher2", Salt = "TeacherSalt456", Password = "teacher!", Email = "teacher2@example.com", FirstName = "Teacher2", LastName = "User", Role = "teacher" },

            // courseAdmin (3 st)
            new { UserName = "courseadmin", Salt = "CourseAdminSalt123", Password = "courseadmin!", Email = "courseadmin@example.com", FirstName = "Course", LastName = "Admin", Role = "courseAdmin" },
            new { UserName = "courseadmin2", Salt = "CourseAdminSalt456", Password = "courseadmin!", Email = "courseadmin2@example.com", FirstName = "Course2", LastName = "Admin", Role = "courseAdmin" },
            new { UserName = "courseadmin3", Salt = "CourseAdminSalt789", Password = "courseadmin!", Email = "courseadmin3@example.com", FirstName = "Course3", LastName = "Admin", Role = "courseAdmin" },

            // student (5 st)
            new { UserName = "user", Salt = "UserSalt123", Password = "student!", Email = "user@example.com", FirstName = "Normal", LastName = "User", Role = "student" },
            new { UserName = "user2", Salt = "UserSalt456", Password = "student!", Email = "user2@example.com", FirstName = "Normal2", LastName = "User", Role = "student" },
            new { UserName = "student3", Salt = "StudentSalt3", Password = "student!", Email = "student3@example.com", FirstName = "Normal3", LastName = "User", Role = "student" },
            new { UserName = "student4", Salt = "StudentSalt4", Password = "student!", Email = "student4@example.com", FirstName = "Normal4", LastName = "User", Role = "student" },
            new { UserName = "student5", Salt = "StudentSalt5", Password = "student!", Email = "student5@example.com", FirstName = "Normal5", LastName = "User", Role = "student" }
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

