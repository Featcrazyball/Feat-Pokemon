using Database;
using Server;
namespace PokemonPocket;

public class Meowth : PokemonMaster
{
    public override float HealthOverride {get;set;} = 40;
    public override string? Requirements { get; set; } = "Level 28";
    public override string? EvolvesTo {get;set;} = "Persian";
    private Meowth() { } //For EF Core
    public Meowth(string nickname, string ownerId) 
    : base("Meowth", "Normal", 40, 45, 35, 40, 40, 90, ownerId, 10, "Pickup")
    {
        Nickname = nickname;
        SkillPool = "Scratch, Growl, Bite, Pay Day, Screech, Fury Swipes, Slash, Toxic, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Meowth(float HP, string nickname, string ownerId, int exp)
    : base("Meowth", "Normal", HP, 45, 35, 40, 40, 90, ownerId, 10, "Pickup")
    {
        Nickname = nickname;
        Experience = exp;
        SkillPool = "Scratch, Growl, Bite, Pay Day, Screech, Fury Swipes, Slash, Toxic, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

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
        if (Level >= 28) {
            using (var context = new DatabaseContext())
            {
                var persian = new Persian(this);
                persian.MaxHealth = persian.HealthOverride;
                persian.EvolveLevelUp(Level-1);

                foreach (var skill in this.Skills)
                {
                    context.Skills.Remove(skill);
                }

                context.PokemonMaster.Remove(this);
                context.PokemonMaster.Add(persian);
                foreach (var skill in persian.Skills)
                {
                    context.Skills.Add(skill);
                }

                // Remove previous and add new Pokemon
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} has evolved from a Meowth to a Persian!");
        } else {
            await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} is not ready to evolve yet.");
        }
    }

    public override async Task GodEvolve(ClientSession session)
    {
        using (var context = new DatabaseContext())
        {
            var persian = new Persian(this);
            persian.MaxHealth = persian.HealthOverride;
            persian.EvolveLevelUp(Level-1);

            foreach (var skill in this.Skills)
            {
                context.Skills.Remove(skill);
            }

            context.PokemonMaster.Remove(this);
            context.PokemonMaster.Add(persian);
            foreach (var skill in persian.Skills)
            {
                context.Skills.Add(skill);
            }

            // Remove previous and add new Pokemon
            context.SaveChanges();
        }
        await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} has evolved from a Meowth to a Persian!");
    }

    public override float calculateDamage(float SkillDamage)
    {
        return SkillDamage;
    }
}
