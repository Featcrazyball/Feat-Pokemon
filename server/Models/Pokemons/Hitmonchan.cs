using Server;
using Database;

namespace PokemonPocket;

public class Hitmonchan : PokemonMaster
{
    public override float HealthOverride {get;set;} = 50;
    public override string? Requirements { get; set; } = "Unevolvable";
    private Hitmonchan() { } //For EF Core
    public Hitmonchan(string nickname, string ownerId) 
    : base("Hitmonchan", "Fighting", 50, 105, 79, 35, 110, 76, ownerId, 20, "Keen Eye")
    {
        Nickname = nickname;
        SkillPool = "Comet Punch, Agility, Fire Punch, Ice Punch, Thunder Punch, Mega Punch, Counter, Seismic Toss, Body Slam, Take Down, Double-Edge, Submission, Rage, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Hitmonchan(float HP, string nickname, string ownerId, int exp)
    : base("Hitmonchan", "Fighting", HP, 105, 79, 35, 110, 76, ownerId, 20, "Keen Eye")
    {
        Nickname = nickname;
        Experience = exp;
        SkillPool = "Comet Punch, Agility, Fire Punch, Ice Punch, Thunder Punch, Mega Punch, Counter, Seismic Toss, Body Slam, Take Down, Double-Edge, Submission, Rage, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

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
        await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} is already at its final evolution stage.");
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}
