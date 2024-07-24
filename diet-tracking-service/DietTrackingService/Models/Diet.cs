using System;

namespace DietTrackingService.Models
{
    public class Diet
    {
        public Guid Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string MealType { get; set; } = string.Empty;
        public string Food { get; set; } = string.Empty;
        public int Calories { get; set; }
        public string? Notes { get; set; } // Make Notes nullable if it can be empty
    }
}
