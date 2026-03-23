using Microsoft.EntityFrameworkCore;
using Registreringstjansten.Models;

namespace Registreringstjansten.Data;

// Database context that manages the connection between the application and the SQLite database
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    public DbSet<Registrering> Registreringar { get; set; }
    public DbSet<StatusHistorik> StatusHistorik { get; set; }
}