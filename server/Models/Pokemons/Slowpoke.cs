using Database;
namespace PokemonPocket;

public class Slowpoke : PokemonMaster
{
    public string? Nickname {get;set;}

    private Slowpoke() { } //For EF Core
    public Slowpoke(string nickname, string ownerId) 
    : base("Slowpoke", "Water/Psychic", 90, 65, 65, 40, 40, 15, ownerId, 20, "Oblivious")
    {
        Nickname = nickname;
    }

    public override void Evolve()
    {
        if (Level >= 37) {
            using (var context = new DatabaseContext())
            {
                var slowbro = new Slowbro(this);
                slowbro.EvolveLevelUp(Level-1); // Level up to current level

                // Remove previous and add new Pokemon
                context.PokemonMaster.Add(slowbro);
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            Console.WriteLine($"{Nickname} has evolved from a Slowpoke to a Slowbro!");
        } else {
            Console.WriteLine($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}