using Database;
namespace PokemonPocket;

public class Mankey : PokemonMaster
{
    public string? Nickname {get;set;}

    private Mankey() { } //For EF Core
    public Mankey(string nickname, string ownerId) 
    : base("Mankey", "Fighting", 40, 80, 35, 35, 45, 70, ownerId, 14, "Vital Spirit")
    {
        Nickname = nickname;
    }

    public override void Evolve()
    {
        if (Level >= 28) {
            using (var context = new DatabaseContext())
            {
                var primeape = new Primeape(this);
                primeape.EvolveLevelUp(Level-1); 

                // Remove previous and add new Pokemon
                context.PokemonMaster.Add(primeape);
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            Console.WriteLine($"{Nickname} has evolved from a Mankey to a Primeape!");
        } else {
            Console.WriteLine($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}