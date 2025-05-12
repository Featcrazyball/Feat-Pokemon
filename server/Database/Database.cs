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
        // public DbSet<Status> Statuses { get; set; } For the time being no need for this
        public DbSet<Skill> Skills { get; set; }

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
    
            modelBuilder.Entity<PokemonMaster>()
                .HasMany(p => p.Skills)
                .WithOne(s => s.Pokemon)
                .HasForeignKey(s => s.PokemonId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Pokemon)
                .WithOne(p => p.Owner)
                .HasForeignKey(p => p.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlite("Data Source=database.db")
                .EnableSensitiveDataLogging()
                .LogTo(Console.WriteLine)
                ;
        }


    }
}