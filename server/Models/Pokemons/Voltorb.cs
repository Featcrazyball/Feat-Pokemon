using Database;
namespace PokemonPocket;

public class Voltorb : PokemonMaster
{
    public string? Nickname {get;set;}

    private Voltorb() { } //For EF Core
    public Voltorb(string nickname, string ownerId) 
    : base("Voltorb", "Electric", 40, 30, 50, 55, 55, 100, ownerId, 20, "Static")
    {
        Nickname = nickname;
    }

    public override void Evolve()
    {
        if (Level >= 30) {
            using (var context = new DatabaseContext())
            {
                var electrode = new Electrode(this);
                electrode.EvolveLevelUp(Level-1); 

                // Remove previous and add new Pokemon
                context.PokemonMaster.Add(electrode);
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            Console.WriteLine($"{Nickname} has evolved from a Voltorb to an Electrode!");
        } else {
            Console.WriteLine($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}