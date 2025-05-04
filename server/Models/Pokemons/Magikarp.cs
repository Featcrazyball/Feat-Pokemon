using Database;
namespace PokemonPocket;

public class Magikarp : PokemonMaster
{
    public string? Nickname {get;set;}

    private Magikarp() { } //For EF Core
    public Magikarp(string nickname, string ownerId) 
    : base("Magikarp", "Water", 20, 10, 55, 15, 20, 80, ownerId, 5, "Splash")
    {
        Nickname = nickname;
    }

    public override void Evolve()
    {
        if (Level >= 16) {
            using (var context = new DatabaseContext())
            {
                var gyarados = new Gyarados(this);
                gyarados.EvolveLevelUp(Level-1);

                // Remove previous and add new Pokemon
                context.PokemonMaster.Add(gyarados);
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            Console.WriteLine($"{Nickname} has evolved from a Magikarp to a Gyarados!");
        } else {
            Console.WriteLine($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}