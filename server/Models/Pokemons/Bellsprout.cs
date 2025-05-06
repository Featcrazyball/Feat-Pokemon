using Database;
using Server;
namespace PokemonPocket;

public class Bellsprout : PokemonMaster
{
    private Bellsprout() { } //For EF Core
    public Bellsprout(string nickname, string ownerId) 
    : base("Bellsprout", "Grass/Poison", 50, 75, 35, 70, 30, 40, ownerId, 10, "Chlorophyll")
    {
        Nickname = nickname;
        SkillPool = "Vine Whip, Growth, Wrap, Sleep Powder, Poison Powder, Stun Spore, Acid, Razor Leaf, Toxic, SolarBeam, Rage, Mimic, Double Team, Bide, Rest, Substitute";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
            foreach (var skill in newSkills) {Skills.Add(skill);};
    }

    public override async Task Evolve(ClientSession session)
    {
        if (Level >= 21) {
            using (var context = new DatabaseContext())
            {
                var weepinbell = new Weepinbell(this);
                weepinbell.EvolveLevelUp(Level-1); 

                // Remove previous and add new Pokemon
                context.PokemonMaster.Add(weepinbell);
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{Nickname} has evolved from a Bellsprout to a Weepinbell!");
        } else {
            await session.SendMessageAsync($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}