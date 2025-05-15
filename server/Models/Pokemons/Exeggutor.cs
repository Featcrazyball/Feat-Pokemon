using Server;
using Database;
namespace PokemonPocket;

public class Exeggutor : PokemonMaster
{
    public override float HealthOverride {get;set;} = 95;
    public override string? Requirements { get; set; } = "Unevolvable";
    private Exeggutor() { } //For EF Core
    public Exeggutor(string nickname, string ownerId) 
    : base("Exeggutor", "Grass/Psychic", 95, 95, 85, 125, 75, 55, ownerId, 30, "Chlorophyll")
    {
        Nickname = nickname;
        SkillPool = "Stomp, Hypnosis, Barrage, Solar Beam, Toxic, Psychic, Rage, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Exeggutor(float HP, string nickname, string ownerId, int exp)
    : base("Exeggutor", "Grass/Psychic", HP, 95, 85, 125, 75, 55, ownerId, 30, "Chlorophyll")
    {
        Nickname = nickname;
        Experience = exp;
        SkillPool = "Stomp, Hypnosis, Barrage, Solar Beam, Toxic, Psychic, Rage, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Exeggutor(Exeggcute exeggcute)
    : base("Exeggcute", "Grass/Psychic", 100, 95, 85, 125, 75, 55, exeggcute.OwnerId ?? "Unknown", 30, "Chlorophyll")
    {
        Id = exeggcute.Id;
        Level = 1;
        Nickname = exeggcute.Nickname;
        Experience = 0;
        HpIV = exeggcute.HpIV;
        AttackIV = exeggcute.AttackIV;
        SpecialAttackIV = exeggcute.SpecialAttackIV;
        DefenseIV = exeggcute.DefenseIV;
        SpecialDefenseIV = exeggcute.SpecialDefenseIV;
        SpeedIV = exeggcute.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;
        SkillPool = "Stomp, Hypnosis, Barrage, Solar Beam, Toxic, Psychic, Rage, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

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
