using Models;
using Database;

namespace PokemonPocket;

public class Abra : PokemonMaster
{
    public string? Nickname {get;set;}

    private Abra() { } //For EF Core
    public Abra(string nickname, string ownerId) 
    : base("Abra", "Psychic", 25, 20, 15, 105, 55, 90, ownerId, 10, "Synchronize")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }

    public override void Evolve()
    {
        if (Level >= 16) {
            using (var context = new DatabaseContext())
            {
                var kadabra = new Kadabra(this);
                kadabra.EvolveLevelUp(Level-1); // Level up to current level

                // Remove previous and add new Pokemon
                context.PokemonMaster.Add(kadabra);
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            Console.WriteLine($"{Nickname} has evolved from a Abra to a Kadabra!");
        } else {
            Console.WriteLine($"{Nickname} is not ready to evolve yet.");
        }
    }
}