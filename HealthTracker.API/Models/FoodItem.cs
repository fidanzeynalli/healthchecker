using System.ComponentModel.DataAnnotations;

namespace HealthTracker.API.Models
{
    public class FoodItem
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string? Brand { get; set; }
        public int Calories { get; set; }
        public int Carbs { get; set; }
        public int Fat { get; set; }
        public int Protein { get; set; }
        public int Sodium { get; set; }
        public int Sugar { get; set; }
        public string? UserId { get; set; } // Kişisel gıdalar için
    }
} 