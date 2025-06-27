using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using HealthTracker.API.Models;

namespace HealthTracker.API.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>

    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
           : base(options)
        {
        }

        public DbSet<Workout> Workouts { get; set; }
        // Nutrition tablosu eklendi
        public DbSet<Nutrition> Nutritions { get; set; }
        // İleride diğer modelleriniz...
        public DbSet<Sleep> Sleeps { get; set; } // Sleep tablosu eklendi
        public DbSet<Step> Steps { get; set; } // Step tablosu eklendi
        // Yeni eklenenler:
        public DbSet<FoodItem> FoodItems { get; set; }
        public DbSet<Meal> Meals { get; set; }
        public DbSet<MealItem> MealItems { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<DailyLog> DailyLogs { get; set; }
        public DbSet<WaterLog> WaterLogs { get; set; }
    }
}