using Microsoft.AspNetCore.Mvc;
using DietTrackingService.Models;
using DietTrackingService.Data;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Net.Http.Json;

namespace DietTrackingService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DietsController : ControllerBase
    {
        private readonly DietContext _dietContext;
        private readonly IHttpClientFactory _httpClientFactory;

        public DietsController(DietContext dietContext, IHttpClientFactory httpClientFactory)
        {
            _dietContext = dietContext;
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost]
        public async Task<ActionResult<Diet>> LogDiet(Diet diet)
        {
            // Check if the UserId is valid
            var client = _httpClientFactory.CreateClient("UserManagement");
            var response = await client.GetAsync($"/api/users/validate/{diet.UserId}");
            if (!response.IsSuccessStatusCode || !await response.Content.ReadFromJsonAsync<bool>())
            {
                return BadRequest(new { message = "Invalid UserId" });
            }

            diet.Id = Guid.NewGuid();
            _dietContext.Diets.Add(diet);
            await _dietContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetDietLogById), new { id = diet.Id }, diet);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Diet>>> GetDietLogs()
        {
            return await _dietContext.Diets.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Diet>> GetDietLogById(Guid id)
        {
            var diet = await _dietContext.Diets.FindAsync(id);
            if (diet == null)
            {
                return NotFound();
            }
            return diet;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDietLog(Guid id, Diet diet)
        {
            if (id != diet.Id)
            {
                return BadRequest();
            }

            _dietContext.Entry(diet).State = EntityState.Modified;

            try
            {
                await _dietContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_dietContext.Diets.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDietLog(Guid id)
        {
            var diet = await _dietContext.Diets.FindAsync(id);
            if (diet == null)
            {
                return NotFound();
            }

            _dietContext.Diets.Remove(diet);
            await _dietContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
