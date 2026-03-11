using Microsoft.EntityFrameworkCore;
//using KommunicationAPI.Models;

namespace KommunicationAPI.Data;

public class KommunicationDbContext : DbContext
{
    public KommunicationDbContext(DbContextOptions<KommunicationDbContext> options)
        : base(options)
    {
    }

    //public DbSet<Message> Messages { get; set; }
}