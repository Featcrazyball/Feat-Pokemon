using Database;
using Server;
namespace PokemonPocket;

public class NidoranF : PokemonMaster
{
    public override string? Requirements { get; set; } = "Level 16";
    private NidoranF() { } //For EF Core
    public NidoranF(string nickname, string ownerId) 
    : base("NidoranF", "Poison", 55, 47, 52, 40, 40, 41, ownerId, 10, "Poison Point")
    {
        Nickname = nickname;

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
            foreach (var skill in newSkills) {Skills.Add(skill);};
    }

    public override async Task Evolve(ClientSession session)
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
            await session.SendMessageAsync($"{Nickname} has evolved from a NidoranF to a Nidorina!");
        } else {
            await session.SendMessageAsync($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}