using Database;
using Server;
namespace PokemonPocket;

public class Diglett : PokemonMaster
{
    public override float HealthOverride {get;set;} = 10;
    public override string? Requirements { get; set; } = "Level 26";
    public override string? EvolvesTo {get;set;} = "Dugtrio";
    private Diglett() { } //For EF Core
    public Diglett(string nickname, string ownerId) 
    : base("Diglett", "Ground", 10, 55, 25, 35, 45, 95, ownerId, 10, "Sand Veil")
    {
        Nickname = nickname;
        SkillPool = "Scratch, Growl, Dig, Sand Attack, Slash, Earthquake, Fissure, Toxic, Body Slam, Take Down, Double-Edge, Rage, Mimic, Double Team, Reflect, Bide, Substitute";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Diglett(float HP, string nickname, string ownerId, int exp)
    : base("Diglett", "Ground", HP, 55, 25, 35, 45, 95, ownerId, 10, "Sand Veil")
    {
        Nickname = nickname;
        Experience = exp;
        SkillPool = "Scratch, Growl, Dig, Sand Attack, Slash, Earthquake, Fissure, Toxic, Body Slam, Take Down, Double-Edge, Rage, Mimic, Double Team, Reflect, Bide, Substitute";

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
        if (Level >= 26) {
            using (var context = new DatabaseContext())
            {
                var dugtrio = new Dugtrio(this);
                dugtrio.MaxHealth = dugtrio.HealthOverride;
                dugtrio.EvolveLevelUp(Level-1);

                foreach (var skill in this.Skills)
                {
                    context.Skills.Remove(skill);
                }

                // Add skills for the evolved Pokemon
                context.PokemonMaster.Remove(this);
                context.PokemonMaster.Add(dugtrio);
                foreach (var skill in dugtrio.Skills)
                {
                    context.Skills.Add(skill);
                }
                
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} has evolved from a Diglett to a Dugtrio!");
        } else {
            await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} is not ready to evolve yet.");
        }
    }
    
    public override async Task GodEvolve(ClientSession session)
    {
        using (var context = new DatabaseContext())
        {
            var dugtrio = new Dugtrio(this);
            dugtrio.MaxHealth = dugtrio.HealthOverride;
            dugtrio.EvolveLevelUp(Level-1);

            foreach (var skill in this.Skills)
            {
                context.Skills.Remove(skill);
            }

            // Add skills for the evolved Pokemon
            context.PokemonMaster.Remove(this);
            context.PokemonMaster.Add(dugtrio);
            foreach (var skill in dugtrio.Skills)
            {
                context.Skills.Add(skill);
            }
            
            context.SaveChanges();
        }
        await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} has evolved from a Diglett to a Dugtrio!");
    }

    public override float calculateDamage(float SkillDamage)
    {
        return SkillDamage;
    }
}
