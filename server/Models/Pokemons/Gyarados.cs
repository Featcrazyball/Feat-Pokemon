using Server;
using Database;

namespace PokemonPocket;
public class Gyarados : PokemonMaster
{
    public override float HealthOverride {get;set;} = 95;
    public override string? Requirements { get; set; } = "Unevolvable";
    private Gyarados() { } //For EF Core
    public Gyarados(string nickname, string ownerId) 
    : base("Gyarados", "Water/Ice", 95, 125, 79, 60, 100, 81, ownerId, 30, "Intimidate")
    {
        Nickname = nickname;
        SkillPool = "Bite, Dragon Rage, Leer, Hydro Pump, Hyper Beam, Toxic, Body Slam, Take Down, Double-Edge, Blizzard, Hyper Beam, Rage, Thunderbolt, Thunder, Mimic, Double Team, Reflect, Bide, Fire Blast, Skull Bash, Rest, Substitute, Surf, Strength";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Gyarados(float HP, string nickname, string ownerId, int exp)
    : base("Gyarados", "Water/Ice", HP, 125, 79, 60, 100, 81, ownerId, 30, "Intimidate")
    {
        Nickname = nickname;
        Experience = exp;
        SkillPool = "Bite, Dragon Rage, Leer, Hydro Pump, Hyper Beam, Toxic, Body Slam, Take Down, Double-Edge, Blizzard, Hyper Beam, Rage, Thunderbolt, Thunder, Mimic, Double Team, Reflect, Bide, Fire Blast, Skull Bash, Rest, Substitute, Surf, Strength";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Gyarados(Magikarp magikarp)
    : base("Gyarados", "Water/Ice", 100, 125, 79, 60, 100, 81, magikarp.OwnerId?? "Unknown", 30, "Intimidate")
    {
        Id = magikarp.Id;
        Level = 1;
        Nickname = magikarp.Nickname;
        Experience = 0;
        HpIV = magikarp.HpIV;
        AttackIV = magikarp.AttackIV;
        SpecialAttackIV = magikarp.SpecialAttackIV;
        DefenseIV = magikarp.DefenseIV;
        SpecialDefenseIV = magikarp.SpecialDefenseIV;
        SpeedIV = magikarp.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;
        SkillPool = "Bite, Dragon Rage, Leer, Hydro Pump, Hyper Beam, Toxic, Body Slam, Take Down, Double-Edge, Blizzard, Hyper Beam, Rage, Thunderbolt, Thunder, Mimic, Double Team, Reflect, Bide, Fire Blast, Skull Bash, Rest, Substitute, Surf, Strength";

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
