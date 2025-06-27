using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HealthTracker.API.Data;
using HealthTracker.API.Models;
using System.Linq;
using System.Threading.Tasks;
using HealthTracker.API.Services;
using System.Security.Claims;

namespace HealthTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MealsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMealService _mealService;
        public MealsController(AppDbContext context, IMealService mealService)
        {
            _context = context;
            _mealService = mealService;
        }

        // GET: api/Meals
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var meals = await _context.Meals.Include(m => m.Items).ThenInclude(i => i.FoodItem).ToListAsync();
            return Ok(meals);
        }

        // GET: api/Meals/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var meal = await _context.Meals.Include(m => m.Items).ThenInclude(i => i.FoodItem).FirstOrDefaultAsync(m => m.Id == id);
            if (meal == null) return NotFound();
            return Ok(meal);
        }

        // GET: api/Meals/by-date
        [HttpGet("by-date")]
        public async Task<IActionResult> GetByDate([FromQuery] string date)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!DateTime.TryParse(date, out var dt)) return BadRequest("Invalid date");
            var meals = await _mealService.GetMealsByDateAsync(dt, userId);
            return Ok(meals);
        }

        // POST: api/Meals
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Meal meal)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var created = await _mealService.AddMealAsync(meal, userId);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        // PUT: api/Meals/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Meal meal)
        {
            if (id != meal.Id) return BadRequest();
            _context.Entry(meal).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Meals/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var meal = await _context.Meals.FindAsync(id);
            if (meal == null) return NotFound();
            _context.Meals.Remove(meal);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // POST: api/Meals/{id}/consume
        [HttpPost("{id}/consume")]
        public async Task<IActionResult> Consume(int id)
        {
            var meal = await _context.Meals.Include(m => m.Items).ThenInclude(i => i.FoodItem).FirstOrDefaultAsync(m => m.Id == id);
            if (meal == null) return NotFound();
            // Burada meal'in Nutrition tablosuna eklenmesi veya başka bir işlem yapılabilir.
            // Örnek: Kullanıcının bugünkü Nutrition kaydına bu meal'in toplam değerlerini ekle.
            return Ok(new { message = "Meal consumed (örnek endpoint)" });
        }
    }
} 