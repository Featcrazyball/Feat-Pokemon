using Server;
using Database;
namespace PokemonPocket;

public class Dragonite : PokemonMaster
{
    public override float HealthOverride {get;set;} = 91;
    public override string? Requirements { get; set; } = "Unevolvable";
    private Dragonite() { } //For EF Core
    public Dragonite(string nickname, string ownerId) 
    : base("Dragonite", "Dragon", 91, 134, 95, 100, 100, 80, ownerId, 60, "Inner Focus")
    {
        Nickname = nickname;
        SkillPool = "Wrap, Leer, Thunder Wave, Agility, Slam, Dragon Rage, Hyper Beam, Toxic, Body Slam, Take Down, Double-Edge, Blizzard, Rage, Thunderbolt, Thunder, Surf, Mimic, Double Team, Reflect, Bide, Fire Blast, Swift, Skull Bash, Rest, Substitute";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Dragonite(float HP, string nickname, string ownerId, int exp)
    : base("Dragonite", "Dragon", HP, 134, 95, 100, 100, 80, ownerId, 60, "Inner Focus")
    {
        Nickname = nickname;
        Experience = exp;
        SkillPool = "Wrap, Leer, Thunder Wave, Agility, Slam, Dragon Rage, Hyper Beam, Toxic, Body Slam, Take Down, Double-Edge, Blizzard, Rage, Thunderbolt, Thunder, Surf, Mimic, Double Team, Reflect, Bide, Fire Blast, Swift, Skull Bash, Rest, Substitute";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Dragonite(Dragonair dragonair)
    : base("Dragonite", "Dragon", 100, 134, 95, 100, 100, 80, dragonair.OwnerId?? "Unknown", 60, "Inner Focus")
    {
        Id = dragonair.Id;
        Level = 1;
        Nickname = dragonair.Nickname;
        Experience = 0;
        HpIV = dragonair.HpIV;
        AttackIV = dragonair.AttackIV;
        SpecialAttackIV = dragonair.SpecialAttackIV;
        DefenseIV = dragonair.DefenseIV;
        SpecialDefenseIV = dragonair.SpecialDefenseIV;
        SpeedIV = dragonair.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;
        SkillPool = "Wrap, Leer, Thunder Wave, Agility, Slam, Dragon Rage, Hyper Beam, Toxic, Body Slam, Take Down, Double-Edge, Blizzard, Rage, Thunderbolt, Thunder, Surf, Mimic, Double Team, Reflect, Bide, Fire Blast, Swift, Skull Bash, Rest, Substitute";

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
