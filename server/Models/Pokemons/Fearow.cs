using Server;
using Database;
namespace PokemonPocket;

public class Fearow : PokemonMaster
{
    public override string? Requirements { get; set; } = "Unevolvable";
    private Fearow() { } //For EF Core
    public Fearow(string nickname, string ownerId) 
    : base("Fearow", "Normal/Flying", 65, 90, 65, 61, 61, 100, ownerId, 25, "Peck")
    {
        Nickname = nickname;
        SkillPool = "Peck, Growl, Leer, Fury Attack, Drill Peck, Agility, Toxic, Body Slam, Take Down, Double-Edge, Rage, Mimic, Double Team, Reflect, Bide, Swift, Rest, Substitute, Fly";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Fearow(Spearow spearow)
    : base("Fearow", "Normal/Flying", 65, 90, 65, 61, 61, 100, spearow.OwnerId ?? "Unknown", 25, "Peck")
    {
        Id= spearow.Id;
        Level = 1;
        Nickname = spearow.Nickname;
        Experience = spearow.Experience;
        HpIV = spearow.HpIV;
        AttackIV = spearow.AttackIV;
        SpecialAttackIV = spearow.SpecialAttackIV;
        DefenseIV = spearow.DefenseIV;
        SpecialDefenseIV = spearow.SpecialDefenseIV;
        SpeedIV = spearow.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;
        SkillPool = "Peck, Growl, Leer, Fury Attack, Drill Peck, Agility, Toxic, Body Slam, Take Down, Double-Edge, Rage, Mimic, Double Team, Reflect, Bide, Swift, Rest, Substitute, Fly";

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
        await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} is already at its final form!");
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}