using Server;
using Database;
namespace PokemonPocket;

public class Sandslash : PokemonMaster
{
    public override float HealthOverride {get;set;} = 75;
    public override string? Requirements { get; set; } = "Unevolvable";
    
    private Sandslash() { } //For EF Core
    public Sandslash(string nickname, string ownerId) 
    : base("Sandslash", "Ground", 75, 100, 110, 45, 55, 65, ownerId, 25, "Sand Attack")
    {
        Nickname = nickname;
        SkillPool = "Scratch, Sand Attack, Slash, Poison Sting, Swift, Fury Swipes, Earthquake, Hyper Beam, Body Slam, Seismic Toss, Toxic, Mimic, Double Team, Reflect, Bide, Rest, Substitute, Cut";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Sandslash(float HP, string nickname, string ownerId, int exp)
    : base("Sandslash", "Ground", HP, 100, 110, 45, 55, 65, ownerId, 25, "Sand Attack")
    {
        Nickname = nickname;
        Experience = exp;
        SkillPool = "Scratch, Sand Attack, Slash, Poison Sting, Swift, Fury Swipes, Earthquake, Hyper Beam, Body Slam, Seismic Toss, Toxic, Mimic, Double Team, Reflect, Bide, Rest, Substitute, Cut";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Sandslash(Sandshrew sandshrew)
    : base("Sandslash", "Ground", 100, 100, 110, 45, 55, 65, sandshrew.OwnerId ?? "Unknown", 25, "Sand Attack")
    {
        Id = sandshrew.Id;
        Level = 1;
        Nickname = sandshrew.Nickname;
        Experience = 0;
        HpIV = sandshrew.HpIV;
        AttackIV = sandshrew.AttackIV;
        SpecialAttackIV = sandshrew.SpecialAttackIV;
        DefenseIV = sandshrew.DefenseIV;
        SpecialDefenseIV = sandshrew.SpecialDefenseIV;
        SpeedIV = sandshrew.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;
        SkillPool = "Scratch, Sand Attack, Slash, Poison Sting, Swift, Fury Swipes, Earthquake, Hyper Beam, Body Slam, Seismic Toss, Toxic, Mimic, Double Team, Reflect, Bide, Rest, Substitute, Cut";

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
