using Server;
using Database;
namespace PokemonPocket;

public class Onix : PokemonMaster
{
    public override float HealthOverride {get;set;} = 35;
    public override string? Requirements { get; set; } = "Unevolvable";
    private Onix() { } //For EF Core
    public Onix(string nickname, string ownerId) 
    : base("Onix", "Rock/Ground", 35, 45, 160, 30, 45, 70, ownerId, 20, "Rock Head")
    {
        Nickname = nickname;
        SkillPool = "Tackle, Screech, Bind, Rock Throw, Rage, Slam, Harden, Earthquake, Toxic, Body Slam, Take Down, Double-Edge, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Onix(float HP, string nickname, string ownerId, int exp)
    : base("Onix", "Rock/Ground", HP, 45, 160, 30, 45, 70, ownerId, 20, "Rock Head")
    {
        Nickname = nickname;
        Experience = exp;
        SkillPool = "Tackle, Screech, Bind, Rock Throw, Rage, Slam, Harden, Earthquake, Toxic, Body Slam, Take Down, Double-Edge, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public override async Task GodEvolve(ClientSession session)
    {
        await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} is already at its final evolution stage.");
    }

    public override async Task Evolve(ClientSession session)
    {
        await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} has reached its final evolution stage.");
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}
