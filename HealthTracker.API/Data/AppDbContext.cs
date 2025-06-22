using Microsoft.EntityFrameworkCore;
using HealthTracker.API.Models;

namespace HealthTracker.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        
        public DbSet<Workout> Workouts { get; set; }

    }
}