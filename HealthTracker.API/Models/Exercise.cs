using System;

namespace HealthTracker.API.Models
{
    public class Exercise
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int? Calories { get; set; }
        public string Type { get; set; } = string.Empty;
        public int? Minutes { get; set; }
        public int? Sets { get; set; }
        public int? Reps { get; set; }
        public int? Weight { get; set; }
        public DateTime Date { get; set; }
        public string UserId { get; set; } = string.Empty;
    }
} 