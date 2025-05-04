using Database;
namespace PokemonPocket;

public class Staryu : PokemonMaster
{
    public string? Nickname {get;set;}

    private Staryu() { } //For EF Core
    public Staryu(string nickname, string ownerId) 
    : base("Staryu", "Water", 30, 45, 55, 70, 55, 85, ownerId, 20, "Illuminate")
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

            var starmie = new Starmie(this);
            starmie.EvolveLevelUp(Level-1); // Level up to current level

            // Remove previous and add new Pokemon
            context.PokemonMaster.Add(starmie);
            context.PokemonMaster.Remove(this);
            context.SaveChanges();
        }
        Console.WriteLine($"{Nickname} has evolved from a Staryu to a Starmie!");
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}