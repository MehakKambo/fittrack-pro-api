using Microsoft.EntityFrameworkCore;
using DietTrackingService.Models;

namespace DietTrackingService.Data
{
    public class DietContext : DbContext
    {
        public DietContext(DbContextOptions<DietContext> options) : base(options) { }

        public DbSet<Diet> Diets { get; set; }
    }
}
