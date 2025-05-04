using Database;
namespace PokemonPocket;

public class Shellder : PokemonMaster
{
    public string? Nickname {get;set;}

    private Shellder() { } //For EF Core
    public Shellder(string nickname, string ownerId) 
    : base("Shellder", "Water", 30, 65, 100, 45, 25, 40, ownerId, 15, "Shell Armor")
    {
        Nickname = nickname;
    }

    public override void Evolve()
    {
        using (var context = new DatabaseContext())
        {
            var item = context.Items.FirstOrDefault(i => i.Name == "Water Stone" && i.OwnerId == OwnerId);
            if (item != null) {
                context.Items.Remove(item);
            } else {
                Console.WriteLine($"{Nickname} needs a Water Stone to evolve!");
                return;
            }

            var cloyster = new Cloyster(this);
            cloyster.EvolveLevelUp(Level-1); // Level up to current level

            // Remove previous and add new Pokemon
            context.PokemonMaster.Add(cloyster);
            context.PokemonMaster.Remove(this);
            context.SaveChanges();
        }
        Console.WriteLine($"{Nickname} has evolved from a Shellder to a Cloyster!");
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}