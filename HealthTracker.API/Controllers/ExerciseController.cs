using HealthTracker.API.Data;
using HealthTracker.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ExerciseController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ExerciseController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/exercise?date=2025-06-26
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Exercise>>> Get([FromQuery] DateTime? date)
        {
            var query = _context.Exercises.AsQueryable();
            if (date.HasValue)
            {
                query = query.Where(e => e.Date.Date == date.Value.Date);
            }
            var exercises = await query.ToListAsync();
            return Ok(exercises);
        }

        // GET: api/exercise/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Exercise>> GetById(int id)
        {
            var exercise = await _context.Exercises.FindAsync(id);
            if (exercise == null)
                return NotFound();
            return Ok(exercise);
        }

        // POST: api/exercise
        [HttpPost]
        public async Task<ActionResult<Exercise>> Add([FromBody] Exercise exercise)
        {
            _context.Exercises.Add(exercise);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = exercise.Id }, exercise);
        }

        // DELETE: api/exercise/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var exercise = await _context.Exercises.FindAsync(id);
            if (exercise == null)
                return NotFound();
            _context.Exercises.Remove(exercise);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
} 