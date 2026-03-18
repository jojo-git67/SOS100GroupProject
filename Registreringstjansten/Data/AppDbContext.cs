using Microsoft.EntityFrameworkCore;
using Registreringstjansten.Models;

namespace Registreringstjansten.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    public DbSet<Registrering> Registreringar { get; set; }
    public DbSet<StatusHistorik> StatusHistorik { get; set; }
}