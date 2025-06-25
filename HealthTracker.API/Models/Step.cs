namespace HealthTracker.API.Models
{
    /// <summary>
    /// Bir adım (step) kaydını temsil eder.
    /// </summary>
    public class Step
    {
        /// <summary>Adım kaydının veritabanı kimliği</summary>
        public int Id { get; set; }

        /// <summary>Kullanıcıya ait Id (opsiyonel, ileride kullanıcıya bağlamak için)</summary>
        public string? UserId { get; set; }

        /// <summary>Günün tarihi</summary>
        public DateTime Date { get; set; }

        /// <summary>Adım sayısı</summary>
        public int Count { get; set; }
    }
} 