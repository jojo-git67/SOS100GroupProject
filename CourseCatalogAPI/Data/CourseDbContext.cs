using Microsoft.EntityFrameworkCore;
using CourseCatalogAPI.Models;


namespace  CourseCatalogAPI.Data;

public class CourseDbContext : DbContext
{
    public CourseDbContext(DbContextOptions<CourseDbContext> options) : base(options)
    {
    }

    public DbSet<Course> Courses { get; set; }
}