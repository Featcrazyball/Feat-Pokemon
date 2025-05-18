using Server;
using Database;
namespace PokemonPocket;

public class Persian : PokemonMaster
{
    public override float HealthOverride {get;set;} = 65;
    public override string? Requirements { get; set; } = "Unevolvable";
    private Persian() { } //For EF Core
    public Persian(string nickname, string ownerId) 
    : base("Persian", "Normal", 65, 70, 60, 65, 65, 115, ownerId, 34, "Limber")
    {
        Nickname = nickname;
        SkillPool = "Scratch, Growl, Bite, Screech, Slash, Hyper Beam, Body Slam, Take Down, Double-Edge, Bubble Beam, Thunderbolt, Thunder, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Persian(float HP, string nickname, string ownerId, int exp)
    : base("Persian", "Normal", HP, 70, 60, 65, 65, 115, ownerId, 34, "Limber")
    {
        Nickname = nickname;
        Experience = exp;
        SkillPool = "Scratch, Growl, Bite, Screech, Slash, Hyper Beam, Body Slam, Take Down, Double-Edge, Bubble Beam, Thunderbolt, Thunder, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Persian(Meowth meowth)
    : base("Persian", "Normal", 100, 70, 60, 65, 65, 115, meowth.OwnerId ?? "Unknown", 34, "Limber")
    {
        Id = meowth.Id;
        Level = 1;
        Nickname = meowth.Nickname;
        Experience = 0;
        HpIV = meowth.HpIV;
        AttackIV = meowth.AttackIV;
        SpecialAttackIV = meowth.SpecialAttackIV;
        DefenseIV = meowth.DefenseIV;
        SpecialDefenseIV = meowth.SpecialDefenseIV;
        SpeedIV = meowth.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;
        SkillPool = "Scratch, Growl, Bite, Screech, Slash, Hyper Beam, Body Slam, Take Down, Double-Edge, Bubble Beam, Thunderbolt, Thunder, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

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
