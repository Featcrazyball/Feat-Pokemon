using Database;
namespace PokemonPocket;

public class Ponyta : PokemonMaster
{
    public string? Nickname {get;set;}

    private Ponyta() { } //For EF Core
    public Ponyta(string nickname, string ownerId) 
    : base("Ponyta", "Fire", 50, 85, 55, 65, 65, 90, ownerId, 20, "Flame Body")
    {
        Nickname = nickname;
    }

    public override void Evolve()
    {
        if (Level >= 40) {
            using (var context = new DatabaseContext())
            {
                var rapidash = new Rapidash(this);
                rapidash.EvolveLevelUp(Level-1); 

                // Remove previous and add new Pokemon
                context.PokemonMaster.Add(rapidash);
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            Console.WriteLine($"{Nickname} has evolved from a Ponyta to a Rapidash!");
        } else {
            Console.WriteLine($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}