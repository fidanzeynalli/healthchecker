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
    public class NutritionController : ControllerBase
    {
        private readonly AppDbContext _context;

        public NutritionController(AppDbContext context)
        {
            _context = context;
        }

        // Tüm beslenme kayıtlarını getirir
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Nutrition>>> GetAll()
        {
            var nutritions = await _context.Nutritions.ToListAsync();
            return Ok(nutritions);
        }

        // Belirli bir beslenme kaydını getirir
        [HttpGet("{id}")]
        public async Task<ActionResult<Nutrition>> GetById(int id)
        {
            var nutrition = await _context.Nutritions.FindAsync(id);
            if (nutrition == null)
                return NotFound();
            return Ok(nutrition);
        }

        // Yeni bir beslenme kaydı ekler
        [HttpPost]
        public async Task<ActionResult<Nutrition>> AddNutrition([FromBody] Nutrition nutrition)
        {
            _context.Nutritions.Add(nutrition);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = nutrition.Id }, nutrition);
        }

        // Var olan bir beslenme kaydını günceller
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Nutrition updated)
        {
            if (id != updated.Id)
                return BadRequest("URL'deki ID ile gövdedeki ID eşleşmiyor.");

            var nutrition = await _context.Nutritions.FindAsync(id);
            if (nutrition == null)
                return NotFound();

            nutrition.Date = updated.Date;
            nutrition.Calories = updated.Calories;
            nutrition.Protein = updated.Protein;
            nutrition.Carbs = updated.Carbs;
            nutrition.Fat = updated.Fat;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // Var olan bir beslenme kaydını siler
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var nutrition = await _context.Nutritions.FindAsync(id);
            if (nutrition == null)
                return NotFound();

            _context.Nutritions.Remove(nutrition);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
} 