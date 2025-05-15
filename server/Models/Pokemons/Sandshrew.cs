using Database;
using Server;
namespace PokemonPocket;

public class Sandshrew : PokemonMaster
{
    public override float HealthOverride {get;set;} = 50;
    public override string? Requirements { get; set; } = "Level 22";
    public override string? EvolvesTo {get;set;} = "Sandslash";
    private Sandshrew() { } //For EF Core
    public Sandshrew(string nickname, string ownerId) 
    : base("Sandshrew", "Ground", 50, 75, 85, 20, 30, 40, ownerId, 25, "Scratch")
    {
        Nickname = nickname;
        SkillPool = "Scratch, Sand Attack, Slash, Poison Sting, Swift, Fury Swipes, Earthquake, Body Slam, Seismic Toss, Toxic, Mimic, Double Team, Reflect, Bide, Rest, Substitute, Cut";
        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Sandshrew(float HP, string nickname, string ownerId, int exp)
    : base("Sandshrew", "Ground", HP, 75, 85, 20, 30, 40, ownerId, 25, "Scratch")
    {
        Nickname = nickname;
        Experience = exp;
        SkillPool = "Scratch, Sand Attack, Slash, Poison Sting, Swift, Fury Swipes, Earthquake, Body Slam, Seismic Toss, Toxic, Mimic, Double Team, Reflect, Bide, Rest, Substitute, Cut";

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
        if (Level >= 22) {
            using (var context = new DatabaseContext())
            {
                var sandslash = new Sandslash(this);
                sandslash.MaxHealth = sandslash.HealthOverride;
                sandslash.EvolveLevelUp(Level-1); // Level up to 22

                foreach (var skill in this.Skills)
                {
                    context.Skills.Remove(skill);
                }

                context.PokemonMaster.Remove(this);
                context.PokemonMaster.Add(sandslash);
                foreach (var skill in sandslash.Skills)
                {
                    context.Skills.Add(skill);
                }

                // Remove previous and add new Pokemon
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} has evolved from a Sandshrew to a Sandslash!");
        } else {
            await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}
