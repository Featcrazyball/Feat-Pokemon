using Database;
namespace PokemonPocket;

public class Squirtle : PokemonMaster
{
    public string? Nickname {get;set;}

    private Squirtle() { } //For EF Core
    public Squirtle(string nickname, string ownerId) 
    : base("Squirtle", "Water", 44, 48, 65, 50, 64, 43, ownerId, 10, "Torrent")
    {
        Nickname = nickname;
    }

    public override void Evolve()
    {
        if (Level >= 16) {
            using (var context = new DatabaseContext())
            {
                var wartortle = new Wartortle(this);
                wartortle.EvolveLevelUp(Level-1); // Level up to 16

                // Remove previous and add new Pokemon
                context.PokemonMaster.Add(wartortle);
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            Console.WriteLine($"{Nickname} has evolved from a Squirtle to a Wartortle!");
        } else {
            Console.WriteLine($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return 2*SkillDamage;
    }
}