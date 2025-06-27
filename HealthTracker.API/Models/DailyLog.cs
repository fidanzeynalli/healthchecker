using System;
using System.ComponentModel.DataAnnotations;

namespace HealthTracker.API.Models
{
    public class DailyLog
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public required string UserId { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public int TotalCalories { get; set; }
        public double TotalCarb { get; set; }
        public double TotalFat { get; set; }
        public double TotalProtein { get; set; }
        public int TotalWaterMl { get; set; } // Günlük toplam su (ml)
    }
} 