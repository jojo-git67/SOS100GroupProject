using Microsoft.EntityFrameworkCore;
using ScheduleService.Models;

namespace ScheduleService.Data
{
    public class ScheduleDbContext : DbContext
    {
        public ScheduleDbContext(DbContextOptions<ScheduleDbContext> options)
            : base(options)
        {
        }

        public DbSet<ScheduleActivity> ScheduleActivities { get; set; }
    }
}