using Server;
using Database;
namespace PokemonPocket;

public class Marowak : PokemonMaster
{
    public override string? Requirements { get; set; } = "Unevolvable";
    private Marowak() { } //For EF Core
    public Marowak(string nickname, string ownerId) 
    : base("Marowak", "Ground", 60, 80, 110, 50, 80, 45, ownerId, 20, "Lightning Rod")
    {
        Nickname = nickname;
        SkillPool = "Bone Club, Growl, Tail Whip, Headbutt, Leer, Focus Energy, Bonemerang, Rage, Earthquake, Hyper Beam, Toxic, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Marowak(Cubone cubone)
    : base("Marowak", "Ground", 60, 80, 110, 50, 80, 45, cubone.OwnerId ?? "Unknown", 20, "Lightning Rod")
    {
        Id = cubone.Id;
        Level = 1;
        Nickname = cubone.Nickname;
        Experience = cubone.Experience;
        HpIV = cubone.HpIV;
        AttackIV = cubone.AttackIV;
        SpecialAttackIV = cubone.SpecialAttackIV;
        DefenseIV = cubone.DefenseIV;
        SpecialDefenseIV = cubone.SpecialDefenseIV;
        SpeedIV = cubone.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;
        SkillPool = "Bone Club, Growl, Tail Whip, Headbutt, Leer, Focus Energy, Bonemerang, Rage, Earthquake, Hyper Beam, Toxic, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

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