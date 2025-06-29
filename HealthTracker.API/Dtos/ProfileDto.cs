namespace HealthTracker.API.Dtos
{
    public class ProfileDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int? Age { get; set; }
        public double? Weight { get; set; }
        public double? Height { get; set; }
    }
} 