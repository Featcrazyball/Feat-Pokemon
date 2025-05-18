using Database;
using Server;
namespace PokemonPocket;

public class Ponyta : PokemonMaster
{
    public override float HealthOverride {get;set;} = 50;
    public override string? Requirements { get; set; } = "Level 40";
    public override string? EvolvesTo {get;set;} = "Rapidash";
    private Ponyta() { } //For EF Core
    public Ponyta(string nickname, string ownerId) 
    : base("Ponyta", "Fire", 50, 85, 55, 65, 65, 90, ownerId, 20, "Flame Body")
    {
        Nickname = nickname;
        SkillPool = "Ember, Tail Whip, Stomp, Growl, Fire Spin, Agility, Fire Blast, Toxic, Body Slam, Take Down, Double-Edge, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Ponyta(float HP, string nickname, string ownerId, int exp)
    : base("Ponyta", "Fire", HP, 85, 55, 65, 65, 90, ownerId, 20, "Flame Body")
    {
        Nickname = nickname;
        Experience = exp;
        SkillPool = "Ember, Tail Whip, Stomp, Growl, Fire Spin, Agility, Fire Blast, Toxic, Body Slam, Take Down, Double-Edge, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

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
        if (Level >= 40) {
            using (var context = new DatabaseContext())
            {
                var rapidash = new Rapidash(this);
                rapidash.MaxHealth = rapidash.HealthOverride;
                rapidash.EvolveLevelUp(Level-1); 

                foreach (var skill in this.Skills)
                {
                    context.Skills.Remove(skill);
                }

                context.PokemonMaster.Remove(this);
                context.PokemonMaster.Add(rapidash);
                foreach (var skill in rapidash.Skills)
                {
                    context.Skills.Add(skill);
                }

                // Remove previous and add new Pokemon
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} has evolved from a Ponyta to a Rapidash!");
        } else {
            await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} is not ready to evolve yet.");
        }
    }
    
    public override async Task GodEvolve(ClientSession session)
    {
        using (var context = new DatabaseContext())
        {
            var rapidash = new Rapidash(this);
            rapidash.MaxHealth = rapidash.HealthOverride;
            rapidash.EvolveLevelUp(Level - 1);

            foreach (var skill in this.Skills)
            {
                context.Skills.Remove(skill);
            }

            context.PokemonMaster.Remove(this);
            context.PokemonMaster.Add(rapidash);
            foreach (var skill in rapidash.Skills)
            {
                context.Skills.Add(skill);
            }

            // Remove previous and add new Pokemon
            context.SaveChanges();
        }
        await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} has evolved from a Ponyta to a Rapidash!");
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}
