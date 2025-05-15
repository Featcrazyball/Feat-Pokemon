using Server;
using Database;
namespace PokemonPocket;

public class Dodrio : PokemonMaster
{
    public override float HealthOverride {get;set;} = 60;
    public override string? Requirements { get; set; } = "Unevolvable";
    private Dodrio() { } //For EF Core
    public Dodrio(string nickname, string ownerId) 
    : base("Dodrio", "Normal/Flying", 60, 110, 70, 60, 60, 110, ownerId, 30, "Early Bird")
    {
        Nickname = nickname;
        SkillPool = "Peck, Growl, Fury Attack, Drill Peck, Rage, Agility, Tri Attack, Toxic, Body Slam, Take Down, Double-Edge, Hyper Beam, Rage, Mimic, Double Team, Reflect, Bide, Rest, Substitute, Fly";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Dodrio(Doduo doduo)
    : base("Dodrio", "Normal/Flying", 60, 110, 70, 60, 60, 110, doduo.OwnerId ?? "Unknown", 30, "Early Bird")
    {
        Id = doduo.Id;
        Level = 1;
        Nickname = doduo.Nickname;
        Experience = doduo.Experience;
        HpIV = doduo.HpIV;
        AttackIV = doduo.AttackIV;
        SpecialAttackIV = doduo.SpecialAttackIV;
        DefenseIV = doduo.DefenseIV;
        SpecialDefenseIV = doduo.SpecialDefenseIV;
        SpeedIV = doduo.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;
        SkillPool = "Peck, Growl, Fury Attack, Drill Peck, Rage, Agility, Tri Attack, Toxic, Body Slam, Take Down, Double-Edge, Hyper Beam, Rage, Mimic, Double Team, Reflect, Bide, Rest, Substitute, Fly";

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
