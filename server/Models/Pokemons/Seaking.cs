using Server;
using Database;
namespace PokemonPocket;

public class Seaking : PokemonMaster
{
    public override float HealthOverride {get;set;} = 80;
    public override string? Requirements { get; set; } = "Unevolvable";
    private Seaking() { } //For EF Core
    public Seaking(string nickname, string ownerId) 
    : base("Seaking", "Water", 80, 92, 65, 65, 80, 68, ownerId, 30, "Swift Swim")
    {
        Nickname = nickname;
        SkillPool = "Peck, Tail Whip, Supersonic, Horn Attack, Fury Attack, Waterfall, Horn Drill, Agility, Surf, Ice Beam, Blizzard, Body Slam, Toxic, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Seaking(float HP, string nickname, string ownerId, int exp)
    : base("Seaking", "Water", HP, 92, 65, 65, 80, 68, ownerId, 30, "Swift Swim")
    {
        Nickname = nickname;
        Experience = exp;
        SkillPool = "Peck, Tail Whip, Supersonic, Horn Attack, Fury Attack, Waterfall, Horn Drill, Agility, Surf, Ice Beam, Blizzard, Body Slam, Toxic, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Seaking(Goldeen goldeen)
    : base("Seaking", "Water", 100, 92, 65, 65, 80, 68, goldeen.OwnerId ?? "Unknown", 30, "Swift Swim")
    {
        Id = goldeen.Id;
        Level = 1;
        Nickname = goldeen.Nickname;
        Experience = 0;
        HpIV = goldeen.HpIV;
        AttackIV = goldeen.AttackIV;
        SpecialAttackIV = goldeen.SpecialAttackIV;
        DefenseIV = goldeen.DefenseIV;
        SpecialDefenseIV = goldeen.SpecialDefenseIV;
        SpeedIV = goldeen.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;
        SkillPool = "Peck, Tail Whip, Supersonic, Horn Attack, Fury Attack, Waterfall, Horn Drill, Agility, Surf, Ice Beam, Blizzard, Body Slam, Toxic, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

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
