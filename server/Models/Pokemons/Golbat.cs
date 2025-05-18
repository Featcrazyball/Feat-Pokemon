using Server;
using Database;
namespace PokemonPocket;

public class Golbat : PokemonMaster
{
    public override float HealthOverride {get;set;} = 75;
    public override string? Requirements { get; set; } = "Unevolvable";
    private Golbat() { } //For EF Core
    public Golbat(string nickname, string ownerId) 
    : base("Golbat", "Poison/Flying", 75, 80, 70, 65, 75, 90, ownerId, 20, "Inner Focus")
    {
        Nickname = nickname;
        SkillPool = "Leech Life, Screech, Bite, Supersonic, Confuse Ray, Wing Attack, Haze, Toxic, Take Down, Double-Edge, Rage, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Golbat(float HP, string nickname, string ownerId, int exp)
    : base("Golbat", "Poison/Flying", HP, 80, 70, 65, 75, 90, ownerId, 20, "Inner Focus")
    {
        Nickname = nickname;
        Experience = exp;
        SkillPool = "Leech Life, Screech, Bite, Supersonic, Confuse Ray, Wing Attack, Haze, Toxic, Take Down, Double-Edge, Rage, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Golbat(Zubat zubat)
    : base("Golbat", "Poison/Flying", 100, 80, 70, 65, 75, 90, zubat.OwnerId ?? "Unknown", 20, "Inner Focus")
    {
        Id = zubat.Id;
        Level = 1;
        Nickname = zubat.Nickname;
        Experience = 0;
        HpIV = zubat.HpIV;
        AttackIV = zubat.AttackIV;
        SpecialAttackIV = zubat.SpecialAttackIV;
        DefenseIV = zubat.DefenseIV;
        SpecialDefenseIV = zubat.SpecialDefenseIV;
        SpeedIV = zubat.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;
        SkillPool = "Leech Life, Screech, Bite, Supersonic, Confuse Ray, Wing Attack, Haze, Toxic, Take Down, Double-Edge, Rage, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

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
