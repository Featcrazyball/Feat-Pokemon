using Database;
namespace PokemonPocket;

public class Krabby : PokemonMaster
{
    public string? Nickname {get;set;}

    private Krabby() { } //For EF Core
    public Krabby(string nickname, string ownerId) 
    : base("Krabby", "Water", 30, 105, 90, 25, 25, 50, ownerId, 10, "Hyper Cutter")
    {
        Nickname = nickname;
    }

    public override void Evolve()
    {
        if (Level >= 28) {
            using (var context = new DatabaseContext())
            {
                var kingler = new Kingler(this);
                kingler.EvolveLevelUp(Level-1); 

                // Remove previous and add new Pokemon
                context.PokemonMaster.Add(kingler);
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            Console.WriteLine($"{Nickname} has evolved from a Krabby to a Kingler!");
        } else {
            Console.WriteLine($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}