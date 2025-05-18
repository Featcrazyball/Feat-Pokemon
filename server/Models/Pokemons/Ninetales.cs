using Server;
using Database;
namespace PokemonPocket;

public class Ninetales : PokemonMaster
{
    public override float HealthOverride {get;set;} = 73;
    public override string? Requirements { get; set; } = "Unevolvable";
    private Ninetales() { } //For EF Core
    public Ninetales(string nickname, string ownerId) 
    : base("Ninetales", "Fire", 73, 76, 75, 81, 100, 100, ownerId, 20, "Flash Fire")
    {
        Nickname = nickname;
        SkillPool = "Ember, Quick Attack, Roar, Confuse Ray, Flamethrower, Fire Spin, Hyper Beam, Fire Blast, Toxic, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Ninetales(float HP, string nickname, string ownerId, int exp)
    : base("Ninetales", "Fire", HP, 76, 75, 81, 100, 100, ownerId, 20, "Flash Fire")
    {
        Nickname = nickname;
        Experience = exp;
        SkillPool = "Ember, Quick Attack, Roar, Confuse Ray, Flamethrower, Fire Spin, Hyper Beam, Fire Blast, Toxic, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Ninetales(Vulpix vulpix)
    : base("Ninetales", "Fire", 100, 76, 75, 81, 100, 100, vulpix.OwnerId ?? "Unknown", 20, "Flash Fire")
    {
        Id = vulpix.Id;
        Level = 1;
        Nickname = vulpix.Nickname;
        Experience = 0;
        HpIV = vulpix.HpIV;
        AttackIV = vulpix.AttackIV;
        SpecialAttackIV = vulpix.SpecialAttackIV;
        DefenseIV = vulpix.DefenseIV;
        SpecialDefenseIV = vulpix.SpecialDefenseIV;
        SpeedIV = vulpix.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;
        SkillPool = "Ember, Quick Attack, Roar, Confuse Ray, Flamethrower, Fire Spin, Hyper Beam, Fire Blast, Toxic, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

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
