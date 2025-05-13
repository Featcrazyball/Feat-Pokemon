using Server;
using Database;
namespace PokemonPocket;

public class Rhydon : PokemonMaster
{
    public override string? Requirements { get; set; } = "Unevolvable";
    private Rhydon() { } //For EF Core
    public Rhydon(string nickname, string ownerId) 
    : base("Rhydon", "Ground/Rock", 105, 130, 120, 45, 45, 40, ownerId, 30, "Lightning Rod")
    {
        Nickname = nickname;
        SkillPool = "Horn Attack, Stomp, Tail Whip, Fury Attack, Horn Drill, Leer, Take Down, Earthquake, Hyper Beam, Body Slam, Seismic Toss, Toxic, Mimic, Double Team, Reflect, Bide, Rest, Substitute, Strength, Surf";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Rhydon(Rhyhorn rhyhorn)
    : base("Rhydon", "Ground/Rock", 105, 130, 120, 45, 45, 40, rhyhorn.OwnerId ?? "Unknown", 30, "Lightning Rod")
    {
        Id = rhyhorn.Id;
        Level = 1;
        Nickname = rhyhorn.Nickname;
        Experience = rhyhorn.Experience;
        HpIV = rhyhorn.HpIV;
        AttackIV = rhyhorn.AttackIV;
        SpecialAttackIV = rhyhorn.SpecialAttackIV;
        DefenseIV = rhyhorn.DefenseIV;
        SpecialDefenseIV = rhyhorn.SpecialDefenseIV;
        SpeedIV = rhyhorn.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;
        SkillPool = "Horn Attack, Stomp, Tail Whip, Fury Attack, Horn Drill, Leer, Take Down, Earthquake, Hyper Beam, Body Slam, Seismic Toss, Toxic, Mimic, Double Team, Reflect, Bide, Rest, Substitute, Strength, Surf";

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