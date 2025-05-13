using Database;
using Server;
namespace PokemonPocket;

public class Koffing : PokemonMaster
{
    public override string? Requirements { get; set; } = "Level 35";
    private Koffing() { } //For EF Core
    public Koffing(string nickname, string ownerId) 
    : base("Koffing", "Poison", 40, 65, 95, 60, 45, 35, ownerId, 35, "Levitate")
    {
        Nickname = nickname;
        SkillPool = "Tackle, Smog, Sludge, SmokeScreen, Self-Destruct, Haze, Explosion, Toxic, Body Slam, Double-Edge, Thunderbolt, Thunder, Mimic, Double Team, Reflect, Bide, Fire Blast, Rest, Substitute";


        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public override async Task Evolve(ClientSession session)
    {
        if (Level >= 35) {
            using (var context = new DatabaseContext())
            {
                var weezing = new Weezing(this);
                weezing.EvolveLevelUp(Level-1);

                foreach (var skill in this.Skills)
                {
                    context.Skills.Remove(skill);
                }

                context.PokemonMaster.Remove(this);
                context.PokemonMaster.Add(weezing);
                foreach (var skill in weezing.Skills)
                {
                    context.Skills.Add(skill);
                }

                // Remove previous and add new Pokemon
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