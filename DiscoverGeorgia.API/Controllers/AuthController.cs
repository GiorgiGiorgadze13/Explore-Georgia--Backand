using DiscoverGeorgia.API.Data;
using DiscoverGeorgia.API.DTOs;
using DiscoverGeorgia.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DiscoverGeorgia.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Test()
        {
            return Ok("Auth Controller works");
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
            {
                return BadRequest("ამ ელ.ფოსტით მომხმარებელი უკვე დარეგისტრირებულია!");
            }

            string passwordHash = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(dto.Password));

            var user = new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PasswordHash = passwordHash,
                PhoneNumber = dto.PhoneNumber,
                DateOfBirth = dto.DateOfBirth,
                Citizenship = dto.Citizenship,
                City = dto.City,
                Address = dto.Address,
                IsDisabledPerson = dto.IsDisabledPerson,
                RoleId = dto.RoleId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "მომხმარებელი წარმატებით დარეგისტრირდა!", userId = user.Id });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            // 1. მოვძებნოთ მომხმარებელი იმეილით
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null)
            {
                return BadRequest("მომხმარებელი ასეთი ელ.ფოსტით ვერ მოიძებნა!");
            }

            // 2. შევამოწმოთ პაროლი (იმავე ჰეშირების პრინციპით)
            string inputPasswordHash = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(dto.Password));
            if (user.PasswordHash != inputPasswordHash)
            {
                return BadRequest("პაროლი არასწორია!");
            }

            // 3. წარმატებული ავტორიზაცია
            return Ok(new
            {
                message = "ავტორიზაცია წარმატებით გაიარეთ!",
                userId = user.Id,
                email = user.Email,
                firstName = user.FirstName,
                lastName = user.LastName,
                roleId = user.RoleId
            });
        }
    }
}