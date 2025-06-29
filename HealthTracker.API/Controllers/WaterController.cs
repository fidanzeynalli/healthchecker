using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HealthTracker.API.Services;
using System.Threading.Tasks;
using System.Security.Claims;
using System;

namespace HealthTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class WaterController : ControllerBase
    {
        private readonly IWaterService _waterService;
        public WaterController(IWaterService waterService)
        {
            _waterService = waterService;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] int amountMl)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new InvalidOperationException("UserId bulunamadı");
            var log = await _waterService.AddWaterLogAsync(amountMl, DateTime.UtcNow, userId);
            return Ok(log);
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string date)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new InvalidOperationException("UserId bulunamadı");
            if (!DateTime.TryParse(date, out var dt)) return BadRequest("Invalid date");
            var total = await _waterService.GetWaterConsumedAsync(dt, userId);
            return Ok(new { date = dt, waterConsumed = total });
        }
    }
} 