using ChroniclesOfDarknessDiceRoller.Server.Model;
using Microsoft.EntityFrameworkCore;

namespace ChroniclesOfDarknessDiceRoller.Server.Data
{
    public class DiceRollerContext : DbContext
    {
        public DiceRollerContext(DbContextOptions<DiceRollerContext> options) : base(options)
        {
        }

        public DbSet<DiceRoll> DiceRolls { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DiceRoll>().HasKey(t => t.Id);
        }
    }
}