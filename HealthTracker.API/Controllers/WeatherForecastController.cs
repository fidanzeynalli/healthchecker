using Microsoft.AspNetCore.Mvc;

namespace HealthTracker.API.Controllers
{   /// <summary>
    /// Egzersiz verileri ile ilgili iþlemleri yöneten controller.
    /// Kullanýcýnýn yaptýðý antrenmanlarý listeleme ve yeni antrenman ekleme iþlevlerini saðlar.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class WorkoutController : ControllerBase
    {
        private static readonly List<string> _workoutList = new()
        {
            "Koþu",
            "Yoga",
            "Aðýrlýk Antrenmaný",
            "Bisiklet Sürme"
        };
        /// <summary>
        /// Sistemde kayýtlý olan tüm antrenman türlerini getirir.
        /// Bu endpoint, kullanýcýnýn egzersiz geçmiþini ya da seçenekleri görmesi için kullanýlýr.
        /// </summary>
        /// <returns>Egzersiz isimlerini içeren bir liste (string)</returns>
        [HttpGet]
        public ActionResult<IEnumerable<string>> GetAllWorkouts()
        {
            return Ok(_workoutList);
        }
        /// <summary>
        /// Sisteme yeni bir antrenman türü ekler.
        /// Örneðin: "Yüzme", "Pilates" gibi.
        /// </summary>
        /// <param name="workout">Eklenmek istenen antrenman adý (örneðin: 'Yüzme')</param>
        /// <returns>Baþarýlý eklendiyse bilgilendirme mesajý döner</returns>
        [HttpPost]
        public ActionResult AddWorkout([FromBody] string workout)
        {
            if (string.IsNullOrWhiteSpace(workout))
                return BadRequest("Antrenman adý boþ olamaz.");

            _workoutList.Add(workout);
            return Ok($"'{workout}' adlý antrenman baþarýyla eklendi.");
        }
    }
}
