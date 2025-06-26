using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HealthTracker.API.Models
{
    public class Meal
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public List<MealItem> Items { get; set; } = new();
    }
} 