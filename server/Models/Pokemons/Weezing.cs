using Server;
using Database;
namespace PokemonPocket;

public class Weezing : PokemonMaster
{
    public override string? Requirements { get; set; } = "Unevolvable";
    private Weezing() { } //For EF Core
    public Weezing(string nickname, string ownerId) 
    : base("Weezing", "Poison", 65, 90, 120, 85, 70, 60, ownerId, 35, "Levitate")
    {
        Nickname = nickname;
        SkillPool = "Tackle, Smog, Sludge, SmokeScreen, Self-Destruct, Haze, Explosion, Toxic, Body Slam, Take Down, Double-Edge, Hyper Beam, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }
    
    public Weezing(Koffing koffing)
    : base("Weezing", "Poison", 65, 90, 120, 85, 70, 60, koffing.OwnerId ?? "Unknown", 35, "Levitate")
    {
        Id = koffing.Id;
        Level = 1;
        Nickname = koffing.Nickname;
        Experience = koffing.Experience;
        HpIV = koffing.HpIV;
        AttackIV = koffing.AttackIV;
        SpecialAttackIV = koffing.SpecialAttackIV;
        DefenseIV = koffing.DefenseIV;
        SpecialDefenseIV = koffing.SpecialDefenseIV;
        SpeedIV = koffing.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;
        SkillPool = "Tackle, Smog, Sludge, Smokescreen, Self-Destruct, Haze, Explosion, Toxic, Body Slam, Take Down, Double-Edge, Hyper Beam, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

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

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}