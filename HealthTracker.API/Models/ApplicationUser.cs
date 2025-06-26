using Microsoft.AspNetCore.Identity;

namespace HealthTracker.API.Models
{
    public class ApplicationUser : IdentityUser
    {
        // İstersen buraya profil adı, doğum tarihi gibi ekstra alanlar ekleyebilirsin
        public string FullName { get; set; } = string.Empty;
        public double? Height { get; set; } // cm
        public double? Weight { get; set; } // kg
        public double? BodyFat { get; set; } // %
    }
}
