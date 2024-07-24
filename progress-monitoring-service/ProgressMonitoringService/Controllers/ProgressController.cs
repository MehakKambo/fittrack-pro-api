using Microsoft.AspNetCore.Mvc;
using ProgressMonitoringService.Models;
using ProgressMonitoringService.Data;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Net.Http.Json;

namespace ProgressMonitoringService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProgressController : ControllerBase
    {
        private readonly ProgressContext _progressContext;
        private readonly IHttpClientFactory _httpClientFactory;

        public ProgressController(ProgressContext progressContext, IHttpClientFactory httpClientFactory)
        {
            _progressContext = progressContext;
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost]
        public async Task<ActionResult<Progress>> LogProgress(Progress progress)
        {
            // Check if the UserId is valid
            var client = _httpClientFactory.CreateClient("UserManagement");
            var response = await client.GetAsync($"/api/users/validate/{progress.UserId}");
            if (!response.IsSuccessStatusCode || !await response.Content.ReadFromJsonAsync<bool>())
            {
                return BadRequest(new { message = "Invalid UserId" });
            }

            progress.Id = Guid.NewGuid();
            _progressContext.ProgressRecords.Add(progress);
            await _progressContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProgressById), new { id = progress.Id }, progress);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Progress>>> GetProgressLogs()
        {
            return await _progressContext.ProgressRecords.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Progress>> GetProgressById(Guid id)
        {
            var progress = await _progressContext.ProgressRecords.FindAsync(id);
            if (progress == null)
            {
                return NotFound();
            }
            return progress;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProgressLog(Guid id, Progress progress)
        {
            if (id != progress.Id)
            {
                return BadRequest();
            }

            _progressContext.Entry(progress).State = EntityState.Modified;

            try
            {
                await _progressContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_progressContext.ProgressRecords.Any(e => e.Id == id))
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
        public async Task<IActionResult> DeleteProgressLog(Guid id)
        {
            var progress = await _progressContext.ProgressRecords.FindAsync(id);
            if (progress == null)
            {
                return NotFound();
            }

            _progressContext.ProgressRecords.Remove(progress);
            await _progressContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
