using System;
using System.ComponentModel.DataAnnotations;

namespace HealthTracker.API.Models
{
    public class WaterLog
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public required string UserId { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public int AmountMl { get; set; }
    }
} 