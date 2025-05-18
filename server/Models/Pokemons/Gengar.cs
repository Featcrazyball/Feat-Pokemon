using System.Data.SQLite;
using Server;
using Database;
namespace PokemonPocket;

public class Gengar : PokemonMaster
{
    public override float HealthOverride {get;set;} = 60;
    public override string? Requirements { get; set; } = "Unevolvable";
    private Gengar() { } //For EF Core
    public Gengar(string nickname, string ownerId) 
    : base("Gengar", "Ghost/Poison", 60, 65, 60, 130, 75, 110, ownerId, 25, "Cursed Body")
    {
        Nickname = nickname;
        SkillPool = "Lick, Confuse Ray, Night Shade, Hypnosis, Dream Eater, Toxic, Body Slam, Seismic Toss, Thunderbolt, Thunder, Psychic, Rage, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Gengar(float HP, string nickname, string ownerId, int exp)
    : base("Gengar", "Ghost/Poison", HP, 65, 60, 130, 75, 110, ownerId, 25, "Cursed Body")
    {
        Nickname = nickname;
        Experience = exp;
        SkillPool = "Lick, Confuse Ray, Night Shade, Hypnosis, Dream Eater, Toxic, Body Slam, Seismic Toss, Thunderbolt, Thunder, Psychic, Rage, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Gengar(Haunter haunter)
    : base("Gengar", "Ghost/Poison", 100, 65, 60, 130, 75, 110, haunter.OwnerId ?? "Unknown", 25, "Cursed Body")
    {
        Id = haunter.Id;
        Level = 1;
        Nickname = haunter.Nickname;
        Experience = 0;
        HpIV = haunter.HpIV;
        AttackIV = haunter.AttackIV;
        SpecialAttackIV = haunter.SpecialAttackIV;
        DefenseIV = haunter.DefenseIV;
        SpecialDefenseIV = haunter.SpecialDefenseIV;
        SpeedIV = haunter.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;
        SkillPool = "Lick, Confuse Ray, Night Shade, Hypnosis, Dream Eater, Toxic, Body Slam, Seismic Toss, Thunderbolt, Thunder, Psychic, Rage, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

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
