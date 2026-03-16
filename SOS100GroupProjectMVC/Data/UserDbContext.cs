using Microsoft.EntityFrameworkCore;
using SOS100GroupProjectMVC.Models;

namespace SOS100GroupProjectMVC.Data;

public class UserDbContext : DbContext
{
    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
    {
    }
    public DbSet<User> Users { get; set; }
    public DbSet<UserCredentials> UserCredentials { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasOne(u => u.Credentials)
            .WithOne(c => c.User)
            .HasForeignKey<User>(u => u.UserName);
    }
}