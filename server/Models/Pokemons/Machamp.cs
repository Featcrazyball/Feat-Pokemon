using Server;
using Database;
namespace PokemonPocket;

public class Machamp : PokemonMaster
{
    public override float HealthOverride {get;set;} = 90;
    public override string? Requirements { get; set; } = "Unevolvable";
    private Machamp() { } //For EF Core
    public Machamp(string nickname, string ownerId) 
    : base("Machamp", "Fighting", 90, 130, 80, 65, 85, 55, ownerId, 20, "No Guard")
    {
        Nickname = nickname;
        SkillPool = "Karate Chop, Low Kick, Leer, Focus Energy, Seismic Toss, Submission, Strength, Earthquake, Hyper Beam, Toxic, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Machamp(float HP, string nickname, string ownerId, int exp)
    : base("Machamp", "Fighting", HP, 130, 80, 65, 85, 55, ownerId, 20, "No Guard")
    {
        Nickname = nickname;
        Experience = exp;
        SkillPool = "Karate Chop, Low Kick, Leer, Focus Energy, Seismic Toss, Submission, Strength, Earthquake, Hyper Beam, Toxic, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Machamp(Machoke machoke)
    : base("Machamp", "Fighting", 100, 130, 80, 65, 85, 55, machoke.OwnerId ?? "Unknown", 20, "No Guard")
    {
        Id = machoke.Id;
        Level = 1;
        Nickname = machoke.Nickname;
        Experience = 0;
        HpIV = machoke.HpIV;
        AttackIV = machoke.AttackIV;
        SpecialAttackIV = machoke.SpecialAttackIV;
        DefenseIV = machoke.DefenseIV;
        SpecialDefenseIV = machoke.SpecialDefenseIV;
        SpeedIV = machoke.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;
        SkillPool = "Karate Chop, Low Kick, Leer, Focus Energy, Seismic Toss, Submission, Strength, Earthquake, Hyper Beam, Toxic, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

        using (var context = new DatabaseContext())
        {
            var newSkills = LearnSkillFromSkillPool();
            if (newSkills != null)
            {
                foreach (var skill in newSkills) 
                {
                    Skills.Add(skill);
                    context.Skills.Add(skill);
                };
                context.SaveChanges();
            }
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
