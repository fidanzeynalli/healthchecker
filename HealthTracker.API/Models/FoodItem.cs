using System.ComponentModel.DataAnnotations;

namespace HealthTracker.API.Models
{
    public class FoodItem
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public required string Name { get; set; }
        public int CaloriesPerServing { get; set; }
        public double Carb { get; set; }
        public double Fat { get; set; }
        public double Protein { get; set; }
        public required string ServingSize { get; set; } // ör: "100g", "1 porsiyon"
        public string? UserId { get; set; } // null ise global, dolu ise kişisel
    }
} 