using Microsoft.AspNetCore.Identity;

namespace HealthTracker.API.Models
{
    public class ApplicationUser : IdentityUser
    {
        // İstersen buraya profil adı, doğum tarihi gibi ekstra alanlar ekleyebilirsin
        public string FullName { get; set; } = string.Empty;
    }
}
