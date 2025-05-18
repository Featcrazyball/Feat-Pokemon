using Database;
using Server;
namespace PokemonPocket;

public class Paras : PokemonMaster
{
    public override float HealthOverride {get;set;} = 35;
    public override string? Requirements { get; set; } = "Level 24";
    public override string? EvolvesTo {get;set;} = "Parasect";
    private Paras() { } //For EF Core
    public Paras(string nickname, string ownerId) 
    : base("Paras", "Bug/Grass", 35, 70, 55, 45, 55, 25, ownerId, 12, "Effect Spore")
    {
        Nickname = nickname;
        SkillPool = "Scratch, Stun Spore, Leech Life, Spore, Slash, Growth, SolarBeam, Toxic, Body Slam, Take Down, Double-Edge, Mimic, Double Team, Reflect, Bide, Rest, Substitute, Cut";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Paras(float HP, string nickname, string ownerId, int exp)
    : base("Paras", "Bug/Grass", HP, 70, 55, 45, 55, 25, ownerId, 12, "Effect Spore")
    {
        Nickname = nickname;
        Experience = exp;
        SkillPool = "Scratch, Stun Spore, Leech Life, Spore, Slash, Growth, SolarBeam, Toxic, Body Slam, Take Down, Double-Edge, Mimic, Double Team, Reflect, Bide, Rest, Substitute, Cut";

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
        if (Level >= 24) {
            using (var context = new DatabaseContext())
            {
                var parasect = new Parasect(this);
                parasect.MaxHealth = parasect.HealthOverride;
                parasect.EvolveLevelUp(Level-1);

                foreach (var skill in this.Skills)
                {
                    context.Skills.Remove(skill);
                }

                context.PokemonMaster.Remove(this);
                context.PokemonMaster.Add(parasect);
                foreach (var skill in parasect.Skills)
                {
                    context.Skills.Add(skill);
                }

                // Remove previous and add new Pokemon
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} has evolved from a Paras to a Parasect!");
        } else {
            await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} is not ready to evolve yet.");
        }
    }
    
    public override async Task GodEvolve(ClientSession session)
    {
        using (var context = new DatabaseContext())
        {
            var parasect = new Parasect(this);
            parasect.MaxHealth = parasect.HealthOverride;
            parasect.EvolveLevelUp(Level-1);

            foreach (var skill in this.Skills)
            {
                context.Skills.Remove(skill);
            }

            context.PokemonMaster.Remove(this);
            context.PokemonMaster.Add(parasect);
            foreach (var skill in parasect.Skills)
            {
                context.Skills.Add(skill);
            }

            // Remove previous and add new Pokemon
            context.SaveChanges();
        }
        await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} has evolved from a Paras to a Parasect!");
    }

    public override float calculateDamage(float SkillDamage)
    {
        return SkillDamage;
    }
}
