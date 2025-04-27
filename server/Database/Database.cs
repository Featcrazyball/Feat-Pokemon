using Microsoft.EntityFrameworkCore;
using Models;
using PokemonPocket;

namespace Database
{
    // For Database context
    public class DatabaseContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<PokemonMaster> PokemonMaster { get; set; }
        public DbSet<Evolution> Evolutions { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<SkillPool> SkillPools { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PokemonMaster>()
                .HasDiscriminator<string>("PokemonType")
                .HasValue<PokemonMaster>("Base")
                .HasValue<Bulbasaur>("Bulbasaur");
                
            modelBuilder.Entity<SkillPool>()
                .HasMany(s => s.Skills)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
            
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=database.db");
        }
    }
}