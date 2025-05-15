using Database;
using Server;
namespace PokemonPocket;

public class Bellsprout : PokemonMaster
{
    public override float HealthOverride {get;set;} = 50;
    public override string? Requirements { get; set; } = "Level 21";
    public override string? EvolvesTo {get;set;} = "Weepinbell";

    private Bellsprout() { } //For EF Core
    public Bellsprout(string nickname, string ownerId) 
    : base("Bellsprout", "Grass/Poison", 50, 75, 35, 70, 30, 40, ownerId, 10, "Chlorophyll")
    {
        Nickname = nickname;
        SkillPool = "Vine Whip, Growth, Wrap, Sleep Powder, Poison Powder, Stun Spore, Acid, Razor Leaf, Toxic, SolarBeam, Rage, Mimic, Double Team, Bide, Rest, Substitute";

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
        if (Level >= 21) {  // assuming level 21 is when Bellsprout evolves
            using (var context = new DatabaseContext())
            {
                var weepinbell = new Weepinbell(this);
                weepinbell.EvolveLevelUp(Level-1);

                foreach (var skill in this.Skills)
                {
                    context.Skills.Remove(skill);
                }

                // Add the evolved Pokemon to the context
                // Remove the original Pokemon
                context.PokemonMaster.Remove(this);
                context.PokemonMaster.Add(weepinbell);
                
                // Add all skills for the evolved Pokemon
                foreach (var skill in weepinbell.Skills)
                {
                    context.Skills.Add(skill);
                }
                
                
                // Save all changes in a single transaction
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} has evolved from a Bellsprout to a Weepinbell!");
        } else {
            await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}
