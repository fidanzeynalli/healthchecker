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
    public class SleepController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SleepController(AppDbContext context)
        {
            _context = context;
        }

        // Tüm uyku kayıtlarını getirir
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sleep>>> GetAll()
        {
            var sleeps = await _context.Sleeps.ToListAsync();
            return Ok(sleeps);
        }

        // Belirli bir uyku kaydını getirir
        [HttpGet("{id}")]
        public async Task<ActionResult<Sleep>> GetById(int id)
        {
            var sleep = await _context.Sleeps.FindAsync(id);
            if (sleep == null)
                return NotFound();
            return Ok(sleep);
        }

        // Yeni bir uyku kaydı ekler
        [HttpPost]
        public async Task<ActionResult<Sleep>> AddSleep([FromBody] Sleep sleep)
        {
            _context.Sleeps.Add(sleep);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = sleep.Id }, sleep);
        }

        // Var olan bir uyku kaydını günceller
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Sleep updated)
        {
            if (id != updated.Id)
                return BadRequest("URL'deki ID ile gövdedeki ID eşleşmiyor.");

            var sleep = await _context.Sleeps.FindAsync(id);
            if (sleep == null)
                return NotFound();

            sleep.Date = updated.Date;
            sleep.DurationHours = updated.DurationHours;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // Var olan bir uyku kaydını siler
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var sleep = await _context.Sleeps.FindAsync(id);
            if (sleep == null)
                return NotFound();

            _context.Sleeps.Remove(sleep);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
} 