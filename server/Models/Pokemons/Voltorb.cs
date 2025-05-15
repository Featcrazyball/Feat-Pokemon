using Database;
using Server;
namespace PokemonPocket;

public class Voltorb : PokemonMaster
{
    public override float HealthOverride {get;set;} = 40;
    public override string? Requirements { get; set; } = "Level 30";
    public override string? EvolvesTo {get;set;} = "Electrode";
    private Voltorb() { } //For EF Core
    public Voltorb(string nickname, string ownerId) 
    : base("Voltorb", "Electric", 40, 30, 50, 55, 55, 100, ownerId, 20, "Static")
    {
        Nickname = nickname;
        SkillPool = "Tackle, Screech, Sonic Boom, Self-Destruct, Light Screen, Swift, Explosion, Thunderbolt, Thunder, Toxic, Mimic, Double Team, Reflect, Bide, Rest, Substitute, Flash";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Voltorb(float HP, string nickname, string ownerId, int exp)
    : base("Voltorb", "Electric", HP, 30, 50, 55, 55, 100, ownerId, 20, "Static")
    {
        Nickname = nickname;
        Experience = exp;
        SkillPool = "Tackle, Screech, Sonic Boom, Self-Destruct, Light Screen, Swift, Explosion, Thunderbolt, Thunder, Toxic, Mimic, Double Team, Reflect, Bide, Rest, Substitute, Flash";

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
        if (Level >= 30) {
            using (var context = new DatabaseContext())
            {
                var electrode = new Electrode(this);
                electrode.MaxHealth = electrode.HealthOverride;
                electrode.EvolveLevelUp(Level-1); 

                foreach (var skill in this.Skills)
                {
                    context.Skills.Remove(skill);
                }

                context.PokemonMaster.Remove(this);
                context.PokemonMaster.Add(electrode);
                foreach (var skill in electrode.Skills)
                {
                    context.Skills.Add(skill);
                }

                // Remove previous and add new Pokemon
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} has evolved from a Voltorb to an Electrode!");
        } else {
            await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}
