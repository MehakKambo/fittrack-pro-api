using System;

namespace ProgressMonitoringService.Models
{
    public class Progress
    {
        public Guid Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string Metric { get; set; } = string.Empty; // e.g., Weight, BodyFatPercentage
        public double Value { get; set; }
        public string Notes { get; set; } = string.Empty;
    }
}
