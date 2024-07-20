using Microsoft.AspNetCore.Mvc;
using WorkoutLoggingService.Models;
using WorkoutLoggingService.Data;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;

namespace WorkoutLoggingService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkoutsController : ControllerBase
    {
        private readonly WorkoutContext _context;

        public WorkoutsController(WorkoutContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<Workout>> LogWorkout(Workout workout)
        {
            workout.Id = Guid.NewGuid();
            _context.Workouts.Add(workout);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetWorkoutLogById), new { id = workout.Id }, workout);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Workout>>> GetWorkoutLogs()
        {
            return await _context.Workouts.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Workout>> GetWorkoutLogById(Guid id)
        {
            var workout = await _context.Workouts.FindAsync(id);
            if (workout == null)
            {
                return NotFound();
            }
            return workout;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWorkoutLog(Guid id, Workout workout)
        {
            if (id != workout.Id)
            {
                return BadRequest();
            }

            _context.Entry(workout).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Workouts.Any(e => e.Id == id))
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
        public async Task<IActionResult> DeleteWorkoutLog(Guid id)
        {
            var workout = await _context.Workouts.FindAsync(id);
            if (workout == null)
            {
                return NotFound();
            }

            _context.Workouts.Remove(workout);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
