using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthTracker.API.Models
{
    public class MealItem
    {
        [Key]
        public int MealItemId { get; set; }
        [Required]
        public int MealId { get; set; }
        [ForeignKey("MealId")]
        public Meal Meal { get; set; } = null!;
        [Required]
        public int FoodItemId { get; set; }
        [ForeignKey("FoodItemId")]
        public FoodItem FoodItem { get; set; } = null!;
        public double Quantity { get; set; } // Kullanıcı girdiği miktar
        public double ServingMultiplier { get; set; } // 1 porsiyon, 0.5 porsiyon vb.
    }
} 