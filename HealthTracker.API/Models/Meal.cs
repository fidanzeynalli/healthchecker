using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HealthTracker.API.Models
{
    public class Meal
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public required string UserId { get; set; }
        [Required]
        public required string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<MealItem> Items { get; set; } = new List<MealItem>();
    }
} 