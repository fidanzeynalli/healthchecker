namespace HealthTracker.API.Models
{
    /// <summary>
    /// Bir egzersiz (workout) kaydını temsil eder.
    /// </summary>
    public class Workout
    {
        /// <summary>Egzersizin veritabanı kimliği</summary>
        public int Id { get; set; }

        /// <summary>Egzersizin adı (örn: Koşu, Yüzme)</summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>Egzersizin süresi (dakika cinsinden)</summary>
        public int DurationMinutes { get; set; }

        /// <summary>Egzersizin yapıldığı tarih</summary>
        public DateTime Date { get; set; }
    }
}
