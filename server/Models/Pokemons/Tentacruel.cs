using Server;
using Database;
namespace PokemonPocket;

public class Tentacruel : PokemonMaster
{
    public override float HealthOverride {get;set;} = 80;
    public override string? Requirements { get; set; } = "Unevolvable";
    private Tentacruel() { } //For EF Core
    public Tentacruel(string nickname, string ownerId) 
    : base("Tentacruel", "Water/Poison", 80, 70, 65, 80, 120, 100, ownerId, 30, "Liquid Ooze")
    {
        Nickname = nickname;
        SkillPool = "Acid, Supersonic, Wrap, Poison Sting, Water Gun, Constrict, Barrier, Screech, Toxic, Bubble Beam, Ice Beam, Blizzard, Hyper Beam, Mimic, Double Team, Reflect, Bide, Rest, Substitute, Surf";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Tentacruel(float HP, string nickname, string ownerId, int exp)
    : base("Tentacruel", "Water/Poison", HP, 70, 65, 80, 120, 100, ownerId, 30, "Liquid Ooze")
    {
        Nickname = nickname;
        Experience = exp;
        SkillPool = "Acid, Supersonic, Wrap, Poison Sting, Water Gun, Constrict, Barrier, Screech, Toxic, Bubble Beam, Ice Beam, Blizzard, Hyper Beam, Mimic, Double Team, Reflect, Bide, Rest, Substitute, Surf";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Tentacruel(Tentacool tentacool)
    : base("Tentacruel", "Water/Poison", 100, 70, 65, 80, 120, 100, tentacool.OwnerId ?? "Unknown", 30, "Liquid Ooze")
    {
        Id = tentacool.Id;
        Level = 1;
        Nickname = tentacool.Nickname;
        Experience = 0;
        HpIV = tentacool.HpIV;
        AttackIV = tentacool.AttackIV;
        SpecialAttackIV = tentacool.SpecialAttackIV;
        DefenseIV = tentacool.DefenseIV;
        SpecialDefenseIV = tentacool.SpecialDefenseIV;
        SpeedIV = tentacool.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;
        SkillPool = "Acid, Supersonic, Wrap, Poison Sting, Water Gun, Constrict, Barrier, Screech, Toxic, Bubble Beam, Ice Beam, Blizzard, Hyper Beam, Mimic, Double Team, Reflect, Bide, Rest, Substitute, Surf";

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
