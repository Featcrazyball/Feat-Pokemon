using Server;
using Database;
namespace PokemonPocket;

public class Hypno : PokemonMaster
{
    public override float HealthOverride {get;set;} = 85;
    public override string? Requirements { get; set; } = "Unevolvable";
    private Hypno() { } //For EF Core
    public Hypno(string nickname, string ownerId) 
    : base("Hypno", "Psychic", 85, 73, 70, 73, 115, 67, ownerId, 30, "Insomnia")
    {
        Nickname = nickname;
        SkillPool = "Pound, Hypnosis, Disable, Confusion, Headbutt, Poison Gas, Psychic, Meditate, Seismic Toss, Body Slam, Take Down, Double-Edge, Submission, Counter, Rage, Thunder Wave, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Hypno(float HP, string nickname, string ownerId, int exp)
    : base("Hypno", "Psychic", HP, 73, 70, 73, 115, 67, ownerId, 30, "Insomnia")
    {
        Nickname = nickname;
        Experience = exp;
        SkillPool = "Pound, Hypnosis, Disable, Confusion, Headbutt, Poison Gas, Psychic, Meditate, Seismic Toss, Body Slam, Take Down, Double-Edge, Submission, Counter, Rage, Thunder Wave, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Hypno(Drowzee drowzee)
    : base("Hypno", "Psychic", 100, 73, 70, 73, 115, 67, drowzee.OwnerId ?? "Unknown", 30, "Insomnia")
    {
        Id = drowzee.Id;
        Level = 1;
        Nickname = drowzee.Nickname;
        Experience = 0;
        HpIV = drowzee.HpIV;
        AttackIV = drowzee.AttackIV;
        SpecialAttackIV = drowzee.SpecialAttackIV;
        DefenseIV = drowzee.DefenseIV;
        SpecialDefenseIV = drowzee.SpecialDefenseIV;
        SpeedIV = drowzee.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;
        SkillPool = "Pound, Hypnosis, Disable, Confusion, Headbutt, Poison Gas, Psychic, Meditate, Seismic Toss, Body Slam, Take Down, Double-Edge, Submission, Counter, Rage, Thunder Wave, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

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
