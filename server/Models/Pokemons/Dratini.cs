using Database;
namespace PokemonPocket;

public class Dratini : PokemonMaster
{
    public string? Nickname {get;set;}

    private Dratini() { } //For EF Core
    public Dratini(string nickname, string ownerId) 
    : base("Dratini", "Dragon", 41, 64, 45, 50, 50, 50, ownerId, 15, "Shed Skin")
    {
        Nickname = nickname;
    }

    public override void Evolve()
    {
        if (Level >= 30) {
            using (var context = new DatabaseContext())
            {
                var dragonair = new Dragonair(this);
                dragonair.EvolveLevelUp(Level-1);

                // Remove previous and add new Pokemon
                context.PokemonMaster.Add(dragonair);
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            Console.WriteLine($"{Nickname} has evolved from a Dratini to a Dragonair!");
        } else {
            Console.WriteLine($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}