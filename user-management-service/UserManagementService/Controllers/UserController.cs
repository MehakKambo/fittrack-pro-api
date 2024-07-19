using Microsoft.AspNetCore.Mvc;
using UserManagementService.Models;
using UserManagementService.Data;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;

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
            user.Id = Guid.NewGuid();
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUserProfile), new { id = user.Id }, user);
        }

        [HttpPost("login")]
        public ActionResult<User> Login(User user)
        {
            var existingUser = _context.Users.FirstOrDefault(u => u.Username == user.Username && u.Password == user.Password);
            if (existingUser == null)
            {
                return Unauthorized();
            }
            return Ok(existingUser);
        }

        [HttpGet("profile/{id}")]
        public async Task<ActionResult<User>> GetUserProfile(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
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
            existingUser.Email = user.Email;
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
