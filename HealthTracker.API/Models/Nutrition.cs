namespace HealthTracker.API.Models
{
    /// <summary>
    /// Bir beslenme (nutrition) kaydını temsil eder.
    /// </summary>
    public class Nutrition
    {
        /// <summary>Beslenme kaydının veritabanı kimliği</summary>
        public int Id { get; set; }

        /// <summary>Kullanıcıya ait Id (opsiyonel, ileride kullanıcıya bağlamak için)</summary>
        public string? UserId { get; set; }

        /// <summary>Günün tarihi</summary>
        public DateTime Date { get; set; }

        /// <summary>Toplam kalori</summary>
        public int Calories { get; set; }

        /// <summary>Toplam protein (gram)</summary>
        public int Protein { get; set; }

        /// <summary>Toplam karbonhidrat (gram)</summary>
        public int Carbs { get; set; }

        /// <summary>Toplam yağ (gram)</summary>
        public int Fat { get; set; }
    }
} 