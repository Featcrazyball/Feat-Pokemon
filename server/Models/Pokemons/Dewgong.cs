using Server;
using Database;
namespace PokemonPocket;

public class Dewgong : PokemonMaster
{
    public override float HealthOverride {get;set;} = 90;
    public override string? Requirements { get; set; } = "Unevolvable";
    private Dewgong() { } //For EF Core
    public Dewgong(string nickname, string ownerId) 
    : base("Dewgong", "Water/Ice", 90, 70, 80, 70, 95, 70, ownerId, 30, "Thick Fat")
    {
        Nickname = nickname;
        SkillPool = "Headbutt, Growl, Aurora Beam, Rest, Take Down, Ice Beam, Agility, Toxic, Body Slam, Double-Edge, Blizzard, Hyper Beam, Rage, Mimic, Double Team, Reflect, Bide, Skull Bash, Rest, Substitute, Surf";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Dewgong(float HP, string nickname, string ownerId, int exp)
    : base("Dewgong", "Water/Ice", HP, 70, 80, 70, 95, 70, ownerId, 30, "Thick Fat")
    {
        Nickname = nickname;
        Experience = exp;
        SkillPool = "Headbutt, Growl, Aurora Beam, Rest, Take Down, Ice Beam, Agility, Toxic, Body Slam, Double-Edge, Blizzard, Hyper Beam, Rage, Mimic, Double Team, Reflect, Bide, Skull Bash, Rest, Substitute, Surf";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Dewgong(Seel seel)
    : base("Dewgong", "Water/Ice", 100, 70, 80, 70, 95, 70, seel.OwnerId ?? "Unknown", 30, "Thick Fat")
    {
        Id = seel.Id;
        Level = 1;
        Nickname = seel.Nickname;
        Experience = 0;
        HpIV = seel.HpIV;
        AttackIV = seel.AttackIV;
        SpecialAttackIV = seel.SpecialAttackIV;
        DefenseIV = seel.DefenseIV;
        SpecialDefenseIV = seel.SpecialDefenseIV;
        SpeedIV = seel.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;
        SkillPool = "Headbutt, Growl, Aurora Beam, Rest, Take Down, Ice Beam, Agility, Toxic, Body Slam, Double-Edge, Blizzard, Hyper Beam, Rage, Mimic, Double Team, Reflect, Bide, Skull Bash, Rest, Substitute, Surf";

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
