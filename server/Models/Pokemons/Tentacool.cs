using Database;
namespace PokemonPocket;

public class Tentacool : PokemonMaster
{
    public string? Nickname {get;set;}

    private Tentacool() { } //For EF Core
    public Tentacool(string nickname, string ownerId) 
    : base("Tentacool", "Water/Poison", 40, 40, 35, 50, 100, 70, ownerId, 10, "Clear Body")
    {
        Nickname = nickname;
    }

    public override void Evolve()
    {
        if (Level >= 30) {
            using (var context = new DatabaseContext())
            {
                var tentacruel = new Tentacruel(this);
                tentacruel.EvolveLevelUp(Level-1); 

                // Remove previous and add new Pokemon
                context.PokemonMaster.Add(tentacruel);
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            Console.WriteLine($"{Nickname} has evolved from a Tentacool to a Tentacruel!");
        } else {
            Console.WriteLine($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}