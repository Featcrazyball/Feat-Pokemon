using Server;
using Database;
namespace PokemonPocket;

public class Electrode : PokemonMaster
{
    public override string? Requirements { get; set; } = "Unevolvable";
    private Electrode() { } //For EF Core
    public Electrode(string nickname, string ownerId) 
    : base("Electrode", "Electric", 60, 50, 70, 80, 80, 150, ownerId, 26, "Static")
    {
        Nickname = nickname;
        SkillPool = "Screech, Sonic Boom, Self-Destruct, Light Screen, Swift, Explosion, Toxic, Take Down, Double-Edge, Rage, Thunderbolt, Thunder, Thunder Wave, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Electrode(Voltorb voltorb)
    : base("Electrode", "Electric", 60, 50, 70, 80, 80, 150, voltorb.OwnerId ?? "Unknown", 30, "Hyper Cutter")
    {
        Id = voltorb.Id;
        Level = 1;
        Nickname = voltorb.Nickname;
        Experience = voltorb.Experience;
        HpIV = voltorb.HpIV;
        AttackIV = voltorb.AttackIV;
        SpecialAttackIV = voltorb.SpecialAttackIV;
        DefenseIV = voltorb.DefenseIV;
        SpecialDefenseIV = voltorb.SpecialDefenseIV;
        SpeedIV = voltorb.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;
        SkillPool = "Screech, Sonic Boom, Self-Destruct, Light Screen, Swift, Explosion, Toxic, Take Down, Double-Edge, Rage, Thunderbolt, Thunder, Thunder Wave, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

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