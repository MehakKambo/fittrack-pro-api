using Microsoft.EntityFrameworkCore;
using ProgressMonitoringService.Models;

namespace ProgressMonitoringService.Data
{
    public class ProgressContext : DbContext
    {
        public ProgressContext(DbContextOptions<ProgressContext> options) : base(options) { }

        public DbSet<Progress> ProgressRecords { get; set; }
    }
}
