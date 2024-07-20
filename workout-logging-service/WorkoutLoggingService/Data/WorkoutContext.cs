using Microsoft.EntityFrameworkCore;
using WorkoutLoggingService.Models;

namespace WorkoutLoggingService.Data
{
    public class WorkoutContext : DbContext
    {
        public WorkoutContext(DbContextOptions<WorkoutContext> options) : base(options) { }

        public DbSet<Workout> Workouts { get; set; }
    }
}
