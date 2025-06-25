namespace HealthTracker.API.Models
{
    /// <summary>
    /// Bir uyku (sleep) kaydını temsil eder.
    /// </summary>
    public class Sleep
    {
        /// <summary>Uyku kaydının veritabanı kimliği</summary>
        public int Id { get; set; }

        /// <summary>Kullanıcıya ait Id (opsiyonel, ileride kullanıcıya bağlamak için)</summary>
        public string? UserId { get; set; }

        /// <summary>Günün tarihi</summary>
        public DateTime Date { get; set; }

        /// <summary>Uyku süresi (saat cinsinden)</summary>
        public double DurationHours { get; set; }
    }
} 