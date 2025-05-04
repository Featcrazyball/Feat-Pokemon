using Database;
using Models;
namespace PokemonPocket;

public class Pikachu : PokemonMaster
{
    public string? Nickname {get;set;}

    private Pikachu() { } //For EF Core
    public Pikachu(string nickname, string ownerId) 
    : base("Pikachu", "Electric", 35, 55, 40, 50, 50, 90, ownerId, 30, "Lightning Bolt")
    {
        Nickname = nickname;
    }

    public override void Evolve()
    {
        using (var context = new DatabaseContext())
        {
            var item = context.Items.FirstOrDefault(i => i.Name == "Thunder Stone" && i.OwnerId == OwnerId);
            if (item != null) {
                context.Items.Remove(item);
            } else {
                Console.WriteLine($"{Nickname} needs a Thunderstone to evolve!");
                return;
            }

            var raichu = new Raichu(this);
            raichu.EvolveLevelUp(Level-1); // Level up to current level

            // Remove previous and add new Pokemon
            context.PokemonMaster.Add(raichu);
            context.PokemonMaster.Remove(this);
            context.SaveChanges();
        }
        Console.WriteLine($"{Nickname} has evolved from a Pikachu to a Raichu!");
    }

    public override float calculateDamage(float SkillDamage) {
        return 3*SkillDamage;
    }
}