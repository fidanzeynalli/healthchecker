using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthTracker.API.Models
{
    public class Profile
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("User")]
        public string UserId { get; set; } = string.Empty; // Foreign key to AspNetUsers

        public ApplicationUser User { get; set; } // Navigation property

        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        [MaxLength(256)]
        public string Email { get; set; } = string.Empty;

        public int? Age { get; set; }
        public double? Weight { get; set; }
        public double? Height { get; set; }

        [MaxLength(512)]
        public string PhotoUrl { get; set; } = string.Empty;
    }
} 