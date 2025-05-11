using Server;
using Database;
namespace PokemonPocket;

public class Nidoking : PokemonMaster
{
    public override string? Requirements { get; set; } = "Unevolvable";
    private Nidoking() { } //For EF Core
    public Nidoking(string nickname, string ownerId)
    : base("Nidoking", "Poison/Ground", 81, 102, 77, 85, 75, 85, ownerId, 30, "Poison Point")
    {
        Nickname = nickname;
        SkillPool = "Horn Attack, Tackle, Poison Sting, Focus Energy, Fury Attack, Horn Drill, Double Kick, Earthquake, Toxic, Body Slam, Take Down, Double-Edge, Ice Beam, Blizzard, Hyper Beam, Thunderbolt, Thunder, Mimic, Double Team, Reflect, Bide, Rest, Fire Blast, Skull Bash, Substitute, Surf, Strength";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Nidoking(Nidorino nidorino)
    : base("Nidoking", "Poison/Ground", 81, 102, 77, 85, 75, 85, nidorino.OwnerId ?? "Unknown", 30, "Poison Point")
    {
        Id = nidorino.Id;
        Level = 1;
        Nickname = nidorino.Nickname;
        Experience = nidorino.Experience;
        HpIV = nidorino.HpIV;
        AttackIV = nidorino.AttackIV;
        SpecialAttackIV = nidorino.SpecialAttackIV;
        DefenseIV = nidorino.DefenseIV;
        SpecialDefenseIV = nidorino.SpecialDefenseIV;
        SpeedIV = nidorino.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;
        SkillPool = "Horn Attack, Tackle, Poison Sting, Focus Energy, Fury Attack, Horn Drill, Double Kick, Earthquake, Toxic, Body Slam, Take Down, Double-Edge, Ice Beam, Blizzard, Hyper Beam, Thunderbolt, Thunder, Mimic, Double Team, Reflect, Bide, Rest, Fire Blast, Skull Bash, Substitute, Surf, Strength";

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
        await session.SendMessageAsync($"{Nickname} is already at its final evolution stage.");
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}