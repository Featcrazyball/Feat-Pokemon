using Server;
using Database;
namespace PokemonPocket;

public class Nidoqueen : PokemonMaster
{
    public override float HealthOverride {get;set;} = 90;
    public override string? Requirements { get; set; } = "Unevolvable";
    private Nidoqueen() { } //For EF Core
    public Nidoqueen(string nickname, string ownerId)
    : base("Nidoqueen", "Poison/Ground", 90, 82, 87, 75, 85, 76, ownerId, 30, "Poison Point")
    {
        Nickname = nickname;
        SkillPool = "Scratch, Tackle, Poison Sting, Focus Energy, Fury Swipes, Body Slam, Earthquake, Toxic, Take Down, Double-Edge, Ice Beam, Blizzard, Hyper Beam, Thunderbolt, Thunder, Mimic, Double Team, Reflect, Bide, Rest, Fire Blast, Skull Bash, Substitute, Surf, Strength";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Nidoqueen(float HP, string nickname, string ownerId, int exp)
    : base("Nidoqueen", "Poison/Ground", HP, 82, 87, 75, 85, 76, ownerId, 30, "Poison Point")
    {
        Nickname = nickname;
        Experience = exp;
        SkillPool = "Scratch, Tackle, Poison Sting, Focus Energy, Fury Swipes, Body Slam, Earthquake, Toxic, Take Down, Double-Edge, Ice Beam, Blizzard, Hyper Beam, Thunderbolt, Thunder, Mimic, Double Team, Reflect, Bide, Rest, Fire Blast, Skull Bash, Substitute, Surf, Strength";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Nidoqueen(Nidorina nidorina)
    : base("Nidoqueen", "Poison/Ground", 100, 82, 87, 75, 85, 76, nidorina.OwnerId ?? "Unknown", 30, "Poison Point")
    {
        Id = nidorina.Id;
        Level = 1;
        Nickname = nidorina.Nickname;
        Experience = 0;
        HpIV = nidorina.HpIV;
        AttackIV = nidorina.AttackIV;
        SpecialAttackIV = nidorina.SpecialAttackIV;
        DefenseIV = nidorina.DefenseIV;
        SpecialDefenseIV = nidorina.SpecialDefenseIV;
        SpeedIV = nidorina.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;
        SkillPool = "Scratch, Tackle, Poison Sting, Focus Energy, Fury Swipes, Body Slam, Earthquake, Toxic, Take Down, Double-Edge, Ice Beam, Blizzard, Hyper Beam, Thunderbolt, Thunder, Mimic, Double Team, Reflect, Bide, Rest, Fire Blast, Skull Bash, Substitute, Surf, Strength";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }
    
    public Nidoqueen(string ownerId)
    : base("Nidoqueen", "Poison/Ground", 100, 82, 87, 75, 85, 76, ownerId, 30, "Poison Point")
    {
        Nickname = "None";
        SkillPool = "Scratch, Tackle, Poison Sting, Focus Energy, Fury Swipes, Body Slam, Earthquake, Toxic, Take Down, Double-Edge, Ice Beam, Blizzard, Hyper Beam, Thunderbolt, Thunder, Mimic, Double Team, Reflect, Bide, Rest, Fire Blast, Skull Bash, Substitute, Surf, Strength";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills)
            {
                Skills.Add(skill);
            }
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
