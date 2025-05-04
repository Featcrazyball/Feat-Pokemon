using Database;
namespace PokemonPocket;

public class Horsea : PokemonMaster
{
    public string? Nickname {get;set;}

    private Horsea() { } //For EF Core
    public Horsea(string nickname, string ownerId) 
    : base("Horsea", "Water", 30, 40, 70, 70, 25, 60, ownerId, 10, "Swift Swim")
    {
        Nickname = nickname;
    }

    public override void Evolve()
    {
        if (Level >= 32) {
            using (var context = new DatabaseContext())
            {
                var seadra = new Seadra(this);
                seadra.EvolveLevelUp(Level-1);

                // Remove previous and add new Pokemon
                context.PokemonMaster.Add(seadra);
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            Console.WriteLine($"{Nickname} has evolved from a Horsea to a Seadra!");
        } else {
            Console.WriteLine($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}