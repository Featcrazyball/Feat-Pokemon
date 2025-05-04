using Database;
namespace PokemonPocket;

public class Vulpix : PokemonMaster
{
    public string? Nickname {get;set;}

    private Vulpix() { } //For EF Core
    public Vulpix(string nickname, string ownerId) 
    : base("Vulpix", "Fire", 38, 41, 40, 50, 65, 65, ownerId, 10, "Flash Fire")
    {
        Nickname = nickname;
    }
    
    public override void Evolve()
    {
        using (var context = new DatabaseContext())
        {
            var item = context.Items.FirstOrDefault(i => i.Name == "Fire Stone" && i.OwnerId == OwnerId);
            if (item != null) {
                context.Items.Remove(item);
            } else {
                Console.WriteLine($"{Nickname} needs a Fire Stone to evolve!");
                return;
            }

            var ninetales = new Ninetales(this);
            ninetales.EvolveLevelUp(Level-1); // Level up to current level

            // Remove previous and add new Pokemon
            context.PokemonMaster.Add(ninetales);
            context.PokemonMaster.Remove(this);
            context.SaveChanges();
        }
        Console.WriteLine($"{Nickname} has evolved from a Vulpix to a Ninetales!");
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}