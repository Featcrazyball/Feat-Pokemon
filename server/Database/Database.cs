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
        public DbSet<Item> Items { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<SkillPool> SkillPools { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var discriminator = modelBuilder.Entity<PokemonMaster>()
                .HasDiscriminator<string>("PokemonType")
                .HasValue<PokemonMaster>("Base");

            // Automatically register all types that inherit from PokemonMaster
            var pokemonTypes = typeof(PokemonMaster).Assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(PokemonMaster)));
            
            foreach (var pokemonType in pokemonTypes)
            {
                modelBuilder.Entity(pokemonType).HasBaseType(typeof(PokemonMaster));
                
                modelBuilder.Entity<PokemonMaster>().HasDiscriminator()
                    .HasValue(pokemonType, pokemonType.Name);
            }
    
            // For skills relationship with skill pools
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