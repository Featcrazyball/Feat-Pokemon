using Server;
using Database;
namespace PokemonPocket;

public class Cloyster : PokemonMaster
{
    public override float HealthOverride {get;set;} = 50;
    public override string? Requirements { get; set; } = "Unevolvable";
    private Cloyster() { } //For EF Core
    public Cloyster(string nickname, string ownerId) 
    : base("Cloyster", "Water/Ice", 50, 95, 180, 85, 45, 70, ownerId, 30, "Shell Armor")
    {
        Nickname = nickname;
        SkillPool = "Clamp, Supersonic, Aurora Beam, Withdraw, Leer, Ice Beam, Spike Cannon, Toxic, Blizzard, Hyper Beam, Rage, Mimic, Double Team, Reflect, Bide, Rest, Substitute, Surf";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Cloyster(Shellder shellder)
    : base("Cloyster", "Water/Ice", 50, 95, 180, 85, 45, 70, shellder.OwnerId ?? "Unknown", 30, "Shell Armor")
    {
        Id = shellder.Id;
        Level = 1;
        Nickname = shellder.Nickname;
        Experience = shellder.Experience;
        HpIV = shellder.HpIV;
        AttackIV = shellder.AttackIV;
        SpecialAttackIV = shellder.SpecialAttackIV;
        DefenseIV = shellder.DefenseIV;
        SpecialDefenseIV = shellder.SpecialDefenseIV;
        SpeedIV = shellder.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;
        SkillPool = "Clamp, Supersonic, Aurora Beam, Withdraw, Leer, Ice Beam, Spike Cannon, Toxic, Blizzard, Hyper Beam, Rage, Mimic, Double Team, Reflect, Bide, Rest, Substitute, Surf";

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
