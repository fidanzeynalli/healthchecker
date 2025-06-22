using Microsoft.AspNetCore.Mvc;

namespace HealthTracker.API.Controllers
{   /// <summary>
    /// Egzersiz verileri ile ilgili i�lemleri y�neten controller.
    /// Kullan�c�n�n yapt��� antrenmanlar� listeleme ve yeni antrenman ekleme i�levlerini sa�lar.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class WorkoutController : ControllerBase
    {
        private static readonly List<string> _workoutList = new()
        {
            "Ko�u",
            "Yoga",
            "A��rl�k Antrenman�",
            "Bisiklet S�rme"
        };
        /// <summary>
        /// Sistemde kay�tl� olan t�m antrenman t�rlerini getirir.
        /// Bu endpoint, kullan�c�n�n egzersiz ge�mi�ini ya da se�enekleri g�rmesi i�in kullan�l�r.
        /// </summary>
        /// <returns>Egzersiz isimlerini i�eren bir liste (string)</returns>
        [HttpGet]
        public ActionResult<IEnumerable<string>> GetAllWorkouts()
        {
            return Ok(_workoutList);
        }
        /// <summary>
        /// Sisteme yeni bir antrenman t�r� ekler.
        /// �rne�in: "Y�zme", "Pilates" gibi.
        /// </summary>
        /// <param name="workout">Eklenmek istenen antrenman ad� (�rne�in: 'Y�zme')</param>
        /// <returns>Ba�ar�l� eklendiyse bilgilendirme mesaj� d�ner</returns>
        [HttpPost]
        public ActionResult AddWorkout([FromBody] string workout)
        {
            if (string.IsNullOrWhiteSpace(workout))
                return BadRequest("Antrenman ad� bo� olamaz.");

            _workoutList.Add(workout);
            return Ok($"'{workout}' adl� antrenman ba�ar�yla eklendi.");
        }
    }
}
