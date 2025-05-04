using Database;
namespace PokemonPocket;

public class NidoranF : PokemonMaster
{
    public string? Nickname {get;set;}

    private NidoranF() { } //For EF Core
    public NidoranF(string nickname, string ownerId) 
    : base("NidoranF", "Poison", 55, 47, 52, 40, 40, 41, ownerId, 10, "Poison Point")
    {
        Nickname = nickname;
    }

    public override void Evolve()
    {
        if (Level >= 16) {
            using (var context = new DatabaseContext())
            {
                var nidorina = new Nidorina(this);
                nidorina.EvolveLevelUp(Level-1);

                // Remove previous and add new Pokemon
                context.PokemonMaster.Add(nidorina);
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            Console.WriteLine($"{Nickname} has evolved from a NidoranF to a Nidorina!");
        } else {
            Console.WriteLine($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}