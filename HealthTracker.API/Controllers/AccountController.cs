using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HealthTracker.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using HealthTracker.API.Data;

namespace HealthTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userMgr;
        private readonly SignInManager<ApplicationUser> _signInMgr;
        private readonly IConfiguration _config;
        private readonly AppDbContext _context;

        public AccountController(
            UserManager<ApplicationUser> userMgr,
            SignInManager<ApplicationUser> signInMgr,
            IConfiguration config,
            AppDbContext context)
        {
            _userMgr = userMgr;
            _signInMgr = signInMgr;
            _config = config;
            _context = context;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (dto.Password != dto.ConfirmPassword)
                return BadRequest(new { Errors = new[] { "Şifreler eşleşmiyor." } });

            var user = new ApplicationUser { UserName = dto.Email, Email = dto.Email };
            var result = await _userMgr.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToArray();
                return BadRequest(new { Errors = errors });
            }

            // Profile creation
            var profile = new Profile
            {
                UserId = user.Id,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Age = dto.Age,
                Weight = dto.Weight,
                Height = dto.Height,
                PhotoUrl = "" // Boş string atanıyor, null değil
            };
            _context.Profiles.Add(profile);
            await _context.SaveChangesAsync();

            // JWT Token
            var jwtSettings = _config.GetSection("JwtSettings");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"] ?? jwtSettings["Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(60);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };
            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );
            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiry = expires
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _userMgr.FindByEmailAsync(dto.Email);
            if (user == null)
                return Unauthorized(new { Error = "Email veya şifre hatalı." });

            var result = await _signInMgr.CheckPasswordSignInAsync(user, dto.Password, false);
            if (!result.Succeeded)
                return Unauthorized(new { Error = "Email veya şifre hatalı." });

            // JWT Token oluştur
            var jwtSettings = _config.GetSection("JwtSettings");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(int.Parse(jwtSettings["ExpiryMinutes"]!));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiry = expires
            });
        }

        [HttpGet("profile")]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<IActionResult> GetProfile()
        {
            var authHeader = Request.Headers["Authorization"].ToString();
            Console.WriteLine("Gelen Authorization: '" + authHeader + "'");
            Console.WriteLine("Authorization Header Uzunluğu: " + authHeader.Length);
            Console.WriteLine("Authorization Header Boş mu: " + string.IsNullOrEmpty(authHeader));

            // Giriş yapan kullanıcının ID'sini al
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            Console.WriteLine("Çözümlenen UserId: " + userId);

            if (userId == null)
                return Unauthorized();

            var user = await _userMgr.FindByIdAsync(userId);
            if (user == null)
                return NotFound();

            return Ok(new
            {
                user.FullName,
                user.Email,
                user.Height,
                user.Weight,
                user.BodyFat,
                user.UserName,
                user.Id
                // user.CreatedAt // Eğer ApplicationUser modelinde varsa ekle
            });
        }

        [HttpGet("cors-test")]
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        public IActionResult CorsTest()
        {
            return Ok("CORS OK!");
        }
    }

    // DTO'lar
    public class RegisterDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public int Age { get; set; }
        public double? Height { get; set; }
        public double? Weight { get; set; }
        public double? BodyFat { get; set; }
    }

    public class LoginDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
