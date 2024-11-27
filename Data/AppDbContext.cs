using Microsoft.EntityFrameworkCore;
using Nutrition.Models;

namespace Nutrition.Data
{
    public class AppDbContext : DbContext
    {
        public IConfiguration _config {  get; set; }
        public AppDbContext(IConfiguration config)
        {
            _config = config;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_config.GetConnectionString("DefaultConnection"));
        }

        public DbSet<User> users { get; set; }
        public DbSet<Account> accounts { get; set; }
        public DbSet<Nutritionist> nutritionists { get; set; }
        public DbSet<NutritionPlan> nutrition_plans { get; set; }
        public DbSet<UserProgress> user_progress { get; set; }
        public DbSet<Schedule> schedules { get; set; } 
    }
}
