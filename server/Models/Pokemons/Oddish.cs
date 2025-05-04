using Database;
namespace PokemonPocket;

public class Oddish : PokemonMaster
{
    public string? Nickname {get;set;}

    private Oddish() { } //For EF Core
    public Oddish(string nickname, string ownerId) 
    : base("Oddish", "Grass/Poison", 45, 50, 55, 75, 65, 30, ownerId, 10, "Chlorophyll")
    {
        Nickname = nickname;
    }

    public override void Evolve()
    {
        if (Level >= 21) {
            using (var context = new DatabaseContext())
            {
                var gloom = new Gloom(this);
                gloom.EvolveLevelUp(Level-1);

                // Remove previous and add new Pokemon
                context.PokemonMaster.Add(gloom);
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            Console.WriteLine($"{Nickname} has evolved from a Oddish to a Gloom!");
        } else {
            Console.WriteLine($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}