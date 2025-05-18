using Server;
using Database;
namespace PokemonPocket;

public class Seadra : PokemonMaster
{
    public override float HealthOverride {get;set;} = 55;
    public override string? Requirements { get; set; } = "Unevolvable";
    private Seadra() { } //For EF Core
    public Seadra(string nickname, string ownerId) 
    : base("Seadra", "Water", 55, 65, 95, 95, 45, 85, ownerId, 25, "Poison Point")
    {
        Nickname = nickname;
        SkillPool = "Bubble, Smokescreen, Leer, Water Gun, Agility, Hydro Pump, Surf, Ice Beam, Blizzard, Body Slam, Toxic, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Seadra(float HP, string nickname, string ownerId, int exp)
    : base("Seadra", "Water", HP, 65, 95, 95, 45, 85, ownerId, 25, "Poison Point")
    {
        Nickname = nickname;
        Experience = exp;
        SkillPool = "Bubble, Smokescreen, Leer, Water Gun, Agility, Hydro Pump, Surf, Ice Beam, Blizzard, Body Slam, Toxic, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Seadra(Horsea horsea)
    : base("Seadra", "Water", 100, 65, 95, 95, 45, 85, horsea.OwnerId ?? "Unknown", 25, "Poison Point")
    {
        Id = horsea.Id;
        Level = 1;
        Nickname = horsea.Nickname;
        Experience = 0;
        HpIV = horsea.HpIV;
        AttackIV = horsea.AttackIV;
        SpecialAttackIV = horsea.SpecialAttackIV;
        DefenseIV = horsea.DefenseIV;
        SpecialDefenseIV = horsea.SpecialDefenseIV;
        SpeedIV = horsea.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;
        SkillPool = "Bubble, Smokescreen, Leer, Water Gun, Agility, Hydro Pump, Surf, Ice Beam, Blizzard, Body Slam, Toxic, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

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
