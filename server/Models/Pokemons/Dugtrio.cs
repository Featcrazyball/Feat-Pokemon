using Server;
using Database;
namespace PokemonPocket;

public class Dugtrio : PokemonMaster
{
    public override float HealthOverride {get;set;} = 35;
    public override string? Requirements { get; set; } = "Unevolvable";
    private Dugtrio() { } //For EF Core
    public Dugtrio(string nickname, string ownerId) 
    : base("Dugtrio", "Ground", 35, 100, 50, 50, 70, 120, ownerId, 26, "Sand Veil")
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

    public Dugtrio(float HP, string nickname, string ownerId, int exp)
    : base("Dugtrio", "Ground", HP, 100, 50, 50, 70, 120, ownerId, 26, "Sand Veil")
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

    public Dugtrio(Diglett diglett)
    : base("Dugtrio", "Ground", 100, 100, 50, 50, 70, 120, diglett.OwnerId ?? "Unknown", 26, "Sand Veil")
    {
        Id = diglett.Id;
        Level = 1;
        Nickname = diglett.Nickname;
        Experience = 0;
        HpIV = diglett.HpIV;
        AttackIV = diglett.AttackIV;
        SpecialAttackIV = diglett.SpecialAttackIV;
        DefenseIV = diglett.DefenseIV;
        SpecialDefenseIV = diglett.SpecialDefenseIV;
        SpeedIV = diglett.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;
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

    public Dugtrio(string ownerId) 
    : base("Dugtrio", "Ground", 100, 100, 50, 50, 70, 120, ownerId, 26, "Sand Veil")
    {
        Nickname = "None";
        Experience = 0;
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
        await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} is already at its final evolution stage.");
    }

    public override async Task GodEvolve(ClientSession session)
    {
        await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} is already at its final evolution stage.");
    }
    
    public override float calculateDamage(float SkillDamage)
    {
        return SkillDamage;
    }
}
