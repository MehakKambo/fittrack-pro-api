using Microsoft.AspNetCore.Mvc;
using UserManagementService.Models;
using UserManagementService.Data;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using BCrypt.Net;

namespace UserManagementService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserContext _context;

        public UserController(UserContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(User user)
        {
            // Convert username to lowercase
            user.Username = user.Username.ToLower();

            // Check if a user with the same username already exists
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == user.Username);
            if (existingUser != null)
            {
                // Return a 409 Conflict response if the username is already taken
                return Conflict(new { message = "Username is already taken" });
            }

            // Hash and salt the password
            var newUser = new User
            {
                Id = Guid.NewGuid(),
                Username = user.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.Password),
            };

            // Proceed with registration
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUserProfile), new { id = newUser.Id }, newUser);
        }

        [HttpPost("login")]
        public ActionResult<User> Login(User user)
        {
            // Convert username to lowercase
            user.Username = user.Username.ToLower();

            var existingUser = _context.Users.FirstOrDefault(u => u.Username == user.Username);
            if (existingUser == null || !BCrypt.Net.BCrypt.Verify(user.Password, existingUser.PasswordHash))
            {
                return Unauthorized();
            }
            return Ok(new {id = existingUser.Id, username = existingUser.Username});
        }

        [HttpGet("profile/{id}")]
        public async Task<ActionResult<User>> GetUserProfile(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(new {id = user.Id, username = user.Username});
        }

        [HttpPut("profile")]
        public async Task<IActionResult> UpdateUserProfile(User user)
        {
            var existingUser = await _context.Users.FindAsync(user.Id);
            if (existingUser == null)
            {
                return NotFound();
            }
            existingUser.Username = user.Username;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("profile/{id}")]
        public async Task<IActionResult> DeleteUserAccount(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
