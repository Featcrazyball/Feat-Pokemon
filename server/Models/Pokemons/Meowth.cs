using Database;
namespace PokemonPocket;

public class Meowth : PokemonMaster
{
    public string? Nickname {get;set;}

    private Meowth() { } //For EF Core
    public Meowth(string nickname, string ownerId) 
    : base("Meowth", "Normal", 40, 45, 35, 40, 40, 90, ownerId, 10, "Pickup")
    {
        Nickname = nickname;
    }

    public override void Evolve()
    {
        if (Level >= 28) {
            using (var context = new DatabaseContext())
            {
                var persian = new Persian(this);
                persian.EvolveLevelUp(Level-1);

                // Remove previous and add new Pokemon
                context.PokemonMaster.Add(persian);
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            Console.WriteLine($"{Nickname} has evolved from a Meowth to a Persian!");
        } else {
            Console.WriteLine($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}