using Server;
using Database;
namespace PokemonPocket;

public class Raichu : PokemonMaster
{
    public override float HealthOverride {get;set;} = 60;
    public override string? Requirements { get; set; } = "Unevolvable";
    
    private Raichu() { } //For EF Core
    public Raichu(string nickname, string ownerId) 
    : base("Raichu", "Electric", 60, 90, 55, 90, 80, 110, ownerId, 25, "Thunderbolt")
    {
        Nickname = nickname;
        SkillPool = "ThunderShock, Thunderbolt, Thunder, Quick Attack, Agility, Slam, Surf, Body Slam, Seismic Toss, Toxic, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Raichu(float HP, string nickname, string ownerId, int exp)
    : base("Raichu", "Electric", HP, 90, 55, 90, 80, 110, ownerId, 25, "Thunderbolt")
    {
        Nickname = nickname;
        Experience = exp;
        SkillPool = "ThunderShock, Thunderbolt, Thunder, Quick Attack, Agility, Slam, Surf, Body Slam, Seismic Toss, Toxic, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Raichu(Pikachu pikachu)
    : base("Raichu", "Electric", 100, 90, 55, 90, 80, 110, pikachu.OwnerId ?? "Unknown", 25, "Thunderbolt")
    {
        Id = pikachu.Id;
        Level = 1;
        Nickname = pikachu.Nickname;
        Experience = 0;
        HpIV = pikachu.HpIV;
        AttackIV = pikachu.AttackIV;
        SpecialAttackIV = pikachu.SpecialAttackIV;
        DefenseIV = pikachu.DefenseIV;
        SpecialDefenseIV = pikachu.SpecialDefenseIV;
        SpeedIV = pikachu.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;
        SkillPool = "ThunderShock, Thunderbolt, Thunder, Quick Attack, Agility, Slam, Surf, Body Slam, Seismic Toss, Toxic, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

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
