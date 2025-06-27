using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HealthTracker.API.Services;
using HealthTracker.API.Models;
using System.Threading.Tasks;
using System.Security.Claims;

namespace HealthTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FoodController : ControllerBase
    {
        private readonly IFoodService _foodService;
        public FoodController(IFoodService foodService)
        {
            _foodService = foodService;
        }

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] string query)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var foods = await _foodService.SearchFoodsAsync(query, userId);
            return Ok(foods);
        }

        [HttpGet("personal")]
        public async Task<IActionResult> GetPersonalFoods()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var foods = await _foodService.GetPersonalFoodsAsync(userId);
            return Ok(foods);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var food = await _foodService.GetFoodByIdAsync(id, userId);
            if (food == null) return NotFound();
            return Ok(food);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] FoodItem food)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var created = await _foodService.AddFoodAsync(food, userId);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }
    }
} 