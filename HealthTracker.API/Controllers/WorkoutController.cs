using HealthTracker.API.Data;
using HealthTracker.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HealthTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class WorkoutController : ControllerBase
    {
        private readonly AppDbContext _context;

        public WorkoutController(AppDbContext context)
        {
            _context = context;
        }

        // 1) GET ALL: Tüm antrenmanlarý listeler
        // GET: api/workout
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Workout>>> GetAll()
        {
            var workouts = await _context.Workouts.ToListAsync();
            return Ok(workouts);
        }

        // 2) GET BY ID: Tek bir antrenmaný getirir
        // GET: api/workout/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Workout>> GetById(int id)
        {
            var workout = await _context.Workouts.FindAsync(id);
            if (workout == null)
                return NotFound();
            return Ok(workout);
        }

        // 3) POST: Yeni bir antrenman ekler
        // POST: api/workout
        [HttpPost]
        public async Task<ActionResult<Workout>> AddWorkout([FromBody] Workout workout)
        {
            _context.Workouts.Add(workout);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = workout.Id }, workout);
        }

        // 4) PUT: Var olan bir antrenmaný günceller
        // PUT: api/workout/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Workout updated)
        {
            if (id != updated.Id)
                return BadRequest("URL’deki ID ile gövdedeki ID eþleþmiyor.");

            var workout = await _context.Workouts.FindAsync(id);
            if (workout == null)
                return NotFound();

            workout.Name = updated.Name;
            workout.DurationMinutes = updated.DurationMinutes;
            workout.Date = updated.Date;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // 5) DELETE: Var olan bir antrenmaný siler
        // DELETE: api/workout/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var workout = await _context.Workouts.FindAsync(id);
            if (workout == null)
                return NotFound();

            _context.Workouts.Remove(workout);
            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
}
