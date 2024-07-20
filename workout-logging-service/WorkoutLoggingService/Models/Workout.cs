using System;

namespace WorkoutLoggingService.Models
{
    public class Workout
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string Type { get; set; }
        public DateTime Date { get; set; }
        public int Duration { get; set; } // Duration in minutes
        public string Intensity { get; set; } // e.g., "Low", "Medium", "High"
        public string Notes { get; set; }
    }
}
