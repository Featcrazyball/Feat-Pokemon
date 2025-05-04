using Database;
namespace PokemonPocket;

public class Kabuto : PokemonMaster
{
    public string? Nickname {get;set;}

    private Kabuto() { } //For EF Core
    public Kabuto(string nickname, string ownerId) 
    : base("Kabuto", "Rock/Water", 30, 80, 90, 55, 45, 55, ownerId, 20, "Battle Armor")
    {
        Nickname = nickname;
    }

    public override void Evolve()
    {
        if (Level >= 40) {
            using (var context = new DatabaseContext())
            {
                var kabuto = new Kabutops(this);
                kabuto.EvolveLevelUp(Level-1);

                // Remove previous and add new Pokemon
                context.PokemonMaster.Add(kabuto);
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            Console.WriteLine($"{Nickname} has evolved from a Kabuto to a Kabutops!");
        } else {
            Console.WriteLine($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}