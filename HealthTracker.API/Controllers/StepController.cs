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
    public class StepController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StepController(AppDbContext context)
        {
            _context = context;
        }

        // Tüm adım kayıtlarını getirir
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Step>>> GetAll()
        {
            var steps = await _context.Steps.ToListAsync();
            return Ok(steps);
        }

        // Belirli bir adım kaydını getirir
        [HttpGet("{id}")]
        public async Task<ActionResult<Step>> GetById(int id)
        {
            var step = await _context.Steps.FindAsync(id);
            if (step == null)
                return NotFound();
            return Ok(step);
        }

        // Yeni bir adım kaydı ekler
        [HttpPost]
        public async Task<ActionResult<Step>> AddStep([FromBody] Step step)
        {
            _context.Steps.Add(step);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = step.Id }, step);
        }

        // Var olan bir adım kaydını günceller
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Step updated)
        {
            if (id != updated.Id)
                return BadRequest("URL'deki ID ile gövdedeki ID eşleşmiyor.");

            var step = await _context.Steps.FindAsync(id);
            if (step == null)
                return NotFound();

            step.Date = updated.Date;
            step.Count = updated.Count;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // Var olan bir adım kaydını siler
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var step = await _context.Steps.FindAsync(id);
            if (step == null)
                return NotFound();

            _context.Steps.Remove(step);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
} 