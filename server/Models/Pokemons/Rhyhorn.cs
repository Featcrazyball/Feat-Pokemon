using Database;
using Server;
namespace PokemonPocket;

public class Rhyhorn : PokemonMaster
{
    private Rhyhorn() { } //For EF Core
    public Rhyhorn(string nickname, string ownerId) 
    : base("Rhyhorn", "Ground/Rock", 80, 85, 95, 30, 30, 25, ownerId, 20, "Lightning Rod")
    {
        Nickname = nickname;

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
            foreach (var skill in newSkills) {Skills.Add(skill);};
    }

    public override async Task Evolve(ClientSession session)
    {
        if (Level >= 42) {
            using (var context = new DatabaseContext())
            {
                var rhydon = new Rhydon(this);
                rhydon.EvolveLevelUp(Level-1);

                // Remove previous and add new Pokemon
                context.PokemonMaster.Add(rhydon);
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{Nickname} has evolved from a Rhyhorn to a Rhydon!");
        } else {
            await session.SendMessageAsync($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}