using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using HealthTracker.API.Data;
using HealthTracker.API.Models;
using HealthTracker.API.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace HealthTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProfileController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProfileController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            var profile = await _context.Profiles.FirstOrDefaultAsync(p => p.UserId == userId);
            if (profile == null)
                return NotFound();

            return Ok(new
            {
                profile.FirstName,
                profile.LastName,
                profile.Email,
                profile.Age,
                profile.Weight,
                profile.Height,
                profile.PhotoUrl
            });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProfile([FromForm] ProfileDto dto, IFormFile photo)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new InvalidOperationException("UserId bulunamadı");

            var profile = await _context.Profiles.FirstOrDefaultAsync(p => p.UserId == userId);
            if (profile == null)
            {
                profile = new Profile { UserId = userId };
                _context.Profiles.Add(profile);
            }

            profile.FirstName = dto.FirstName;
            profile.LastName = dto.LastName;
            profile.Email = dto.Email;
            profile.Age = dto.Age;
            profile.Weight = dto.Weight;
            profile.Height = dto.Height;

            if (photo != null && photo.Length > 0)
            {
                var uploadsDir = Path.Combine(_env.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsDir))
                    Directory.CreateDirectory(uploadsDir);

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(photo.FileName)}";
                var filePath = Path.Combine(uploadsDir, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await photo.CopyToAsync(stream);
                }

                profile.PhotoUrl = $"/uploads/{fileName}";
            }

            await _context.SaveChangesAsync();
            return Ok(profile);
        }
    }
} 