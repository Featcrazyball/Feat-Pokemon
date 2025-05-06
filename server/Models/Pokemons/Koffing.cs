using Database;
using Server;
namespace PokemonPocket;

public class Koffing : PokemonMaster
{
    private Koffing() { } //For EF Core
    public Koffing(string nickname, string ownerId) 
    : base("Koffing", "Poison", 40, 65, 95, 60, 45, 35, ownerId, 35, "Levitate")
    {
        Nickname = nickname;

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
            foreach (var skill in newSkills) {Skills.Add(skill);};
    }

    public override async Task Evolve(ClientSession session)
    {
        if (Level >= 35) {
            using (var context = new DatabaseContext())
            {
                var weezing = new Weezing(this);
                weezing.EvolveLevelUp(Level-1);

                // Remove previous and add new Pokemon
                context.PokemonMaster.Add(weezing);
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{Nickname} has evolved from a Koffing to a Weezing!");
        } else {
            await session.SendMessageAsync($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}