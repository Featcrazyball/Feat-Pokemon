using Database;
namespace PokemonPocket;

public class NidoranM : PokemonMaster
{
    public string? Nickname {get;set;}

    private NidoranM() { } //For EF Core
    public NidoranM(string nickname, string ownerId) 
    : base("NidoranM", "Poison", 46, 57, 40, 40, 40, 50, ownerId, 10, "Poison Point")
    {
        Nickname = nickname;
    }

    public override void Evolve()
    {
        if (Level >= 16) {
            using (var context = new DatabaseContext())
            {
                var nidorino = new Nidorino(this);
                nidorino.EvolveLevelUp(Level-1);

                // Remove previous and add new Pokemon
                context.PokemonMaster.Add(nidorino);
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            Console.WriteLine($"{Nickname} has evolved from a NidoranM to a Nidorino!");
        } else {
            Console.WriteLine($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}