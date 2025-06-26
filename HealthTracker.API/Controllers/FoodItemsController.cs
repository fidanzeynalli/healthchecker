using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HealthTracker.API.Data;
using HealthTracker.API.Models;
using System.Linq;
using System.Threading.Tasks;

namespace HealthTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FoodItemsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public FoodItemsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/FoodItems
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var foods = await _context.FoodItems.ToListAsync();
            return Ok(foods);
        }

        // GET: api/FoodItems/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var food = await _context.FoodItems.FindAsync(id);
            if (food == null) return NotFound();
            return Ok(food);
        }

        // POST: api/FoodItems
        [HttpPost]
        public async Task<IActionResult> Create(FoodItem food)
        {
            _context.FoodItems.Add(food);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = food.Id }, food);
        }

        // PUT: api/FoodItems/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, FoodItem food)
        {
            if (id != food.Id) return BadRequest();
            _context.Entry(food).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/FoodItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var food = await _context.FoodItems.FindAsync(id);
            if (food == null) return NotFound();
            _context.FoodItems.Remove(food);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
} 