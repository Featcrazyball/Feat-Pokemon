using Server;
using Database;
namespace PokemonPocket;

public class Magneton : PokemonMaster
{
    public override float HealthOverride {get;set;} = 50;
    public override string? Requirements { get; set; } = "Unevolvable";
    
    private Magneton() { } //For EF Core
    public Magneton(string nickname, string ownerId) 
    : base("Magnetron", "Electric/Steel", 50, 60, 95, 120, 70, 70, ownerId, 20, "Magnet Pull")
    {
        Nickname = nickname;
        SkillPool = "Tackle, Sonic Boom, ThunderShock, Supersonic, Thunder Wave, Thunderbolt, Reflect, Hyper Beam, Toxic, Mimic, Double Team, Bide, Rest, Substitute";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Magneton(float HP, string nickname, string ownerId, int exp)
    : base("Magnetron", "Electric/Steel", HP, 60, 95, 120, 70, 70, ownerId, 20, "Magnet Pull")
    {
        Nickname = nickname;
        Experience = exp;
        SkillPool = "Tackle, Sonic Boom, ThunderShock, Supersonic, Thunder Wave, Thunderbolt, Reflect, Hyper Beam, Toxic, Mimic, Double Team, Bide, Rest, Substitute";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Magneton(Magnemite magnemite)
    : base("Magneton", "Electric/Steel", 50, 60, 95, 120, 70, 70, magnemite.OwnerId ?? "Unknown", 20, "Magnet Pull")
    {
        Id = magnemite.Id;
        Level = 1;
        Nickname = magnemite.Nickname;
        Experience = magnemite.Experience;
        HpIV = magnemite.HpIV;
        AttackIV = magnemite.AttackIV;
        SpecialAttackIV = magnemite.SpecialAttackIV;
        DefenseIV = magnemite.DefenseIV;
        SpecialDefenseIV = magnemite.SpecialDefenseIV;
        SpeedIV = magnemite.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;
        SkillPool = "Tackle, Sonic Boom, ThunderShock, Supersonic, Thunder Wave, Thunderbolt, Reflect, Hyper Beam, Toxic, Mimic, Double Team, Bide, Rest, Substitute";

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
