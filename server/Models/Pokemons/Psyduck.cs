using Database;
namespace PokemonPocket;

public class Psyduck : PokemonMaster
{
    public string? Nickname {get;set;}

    private Psyduck() { } //For EF Core
    public Psyduck(string nickname, string ownerId) 
    : base("Psyduck", "Water", 50, 52, 48, 65, 50, 55, ownerId, 33, "Damp")
    {
        Nickname = nickname;
    }

    public override void Evolve()
    {
        if (Level >= 33) {
            using (var context = new DatabaseContext())
            {
                var golduck = new Golduck(this);
                golduck.EvolveLevelUp(Level-1);

                // Remove previous and add new Pokemon
                context.PokemonMaster.Add(golduck);
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            Console.WriteLine($"{Nickname} has evolved from a Psyduck to a Golduck!");
        } else {
            Console.WriteLine($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}