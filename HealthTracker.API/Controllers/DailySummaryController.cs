using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HealthTracker.API.Data;
using System.Security.Claims;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace HealthTracker.API.Controllers
{
    [ApiController]
    [Route("api/daily-summary")]
    [Authorize]
    public class DailySummaryController : ControllerBase
    {
        private readonly AppDbContext _context;
        public DailySummaryController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get([FromQuery] string date)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new InvalidOperationException("UserId bulunamadı");
            if (!DateTime.TryParse(date, out var dt)) return BadRequest("Invalid date");
            var log = _context.DailyLogs.FirstOrDefault(l => l.UserId == userId && l.Date == dt.Date);
            if (log == null)
            {
                return Ok(new
                {
                    date = dt.Date,
                    caloriesConsumed = 0,
                    caloriesRemaining = 0,
                    macros = new { carb = 0, fat = 0, protein = 0 },
                    waterConsumed = 0
                });
            }
            int goalCalories = 2000; // TODO: User tablosundan oku
            int caloriesRemaining = goalCalories - log.TotalCalories;
            return Ok(new
            {
                date = log.Date,
                caloriesConsumed = log.TotalCalories,
                caloriesRemaining = caloriesRemaining,
                macros = new { carb = log.TotalCarb, fat = log.TotalFat, protein = log.TotalProtein },
                waterConsumed = log.TotalWaterMl
            });
        }
    }
} 