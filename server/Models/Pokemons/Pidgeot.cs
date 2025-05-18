using Server;
using Database;
namespace PokemonPocket;

public class Pidgeot : PokemonMaster
{
    public override float HealthOverride {get;set;} = 83;
    public override string? Requirements { get; set; } = "Unevolvable";
    private Pidgeot() { } //For EF Core
    public Pidgeot(string nickname, string ownerId) 
    : base("Pidgeot", "Normal/Flying", 83, 80, 75, 70, 70, 101, ownerId, 25, "Keen Eye")
    {
        Nickname = nickname;
        SkillPool = "Gust, Sand Attack, Quick Attack, Whirlwind, Wing Attack, Agility, Sky Attack, Hyper Beam, Toxic, Mimic, Double Team, Reflect, Bide, Rest, Substitute, Fly";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Pidgeot(float HP, string nickname, string ownerId, int exp)
    : base("Pidgeot", "Normal/Flying", HP, 80, 75, 70, 70, 101, ownerId, 25, "Keen Eye")
    {
        Nickname = nickname;
        Experience = exp;
        SkillPool = "Gust, Sand Attack, Quick Attack, Whirlwind, Wing Attack, Agility, Sky Attack, Hyper Beam, Toxic, Mimic, Double Team, Reflect, Bide, Rest, Substitute, Fly";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Pidgeot(Pidgeotto pidgeotto)
    : base("Pidgeot", "Normal/Flying", 100, 80, 75, 70, 70, 101, pidgeotto.OwnerId ?? "Unknown", 25, "Keen Eye")
    {
        Id = pidgeotto.Id;
        Level = 1;
        Nickname = pidgeotto.Nickname;
        Experience = 0;
        HpIV = pidgeotto.HpIV;
        AttackIV = pidgeotto.AttackIV;
        SpecialAttackIV = pidgeotto.SpecialAttackIV;
        DefenseIV = pidgeotto.DefenseIV;
        SpecialDefenseIV = pidgeotto.SpecialDefenseIV;
        SpeedIV = pidgeotto.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;
        SkillPool = "Gust, Sand Attack, Quick Attack, Whirlwind, Wing Attack, Agility, Sky Attack, Hyper Beam, Toxic, Mimic, Double Team, Reflect, Bide, Rest, Substitute, Fly";

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
        await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} is already at its final form!");
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}
