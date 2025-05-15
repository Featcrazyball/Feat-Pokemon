using Server;
using Database;
namespace PokemonPocket;

public class Poliwrath : PokemonMaster
{
    public override float HealthOverride {get;set;} = 90;
    public override string? Requirements { get; set; } = "Unevolvable";
    private Poliwrath() { } //For EF Core
    public Poliwrath(string nickname, string ownerId) 
    : base("Poliwrath", "Water", 90, 95, 95, 70, 90, 70, ownerId, 60, "Water Absorb")
    {
        Nickname = nickname;
        SkillPool = "Hypnosis, Water Gun, DoubleSlap, Body Slam, Amnesia, Hydro Pump, Submission, Counter, Seismic Toss, Strength, Earthquake, Toxic, Take Down, Double-Edge, Ice Beam, Blizzard, Hyper Beam, Mimic, Double Team, Reflect, Bide, Rest, Substitute, Surf";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Poliwrath(float HP, string nickname, string ownerId, int exp)
    : base("Poliwrath", "Water", HP, 95, 95, 70, 90, 70, ownerId, 60, "Water Absorb")
    {
        Nickname = nickname;
        Experience = exp;
        SkillPool = "Hypnosis, Water Gun, DoubleSlap, Body Slam, Amnesia, Hydro Pump, Submission, Counter, Seismic Toss, Strength, Earthquake, Toxic, Take Down, Double-Edge, Ice Beam, Blizzard, Hyper Beam, Mimic, Double Team, Reflect, Bide, Rest, Substitute, Surf";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Poliwrath(Poliwhirl poliwhirl)
    : base("Poliwrath", "Water", 100, 95, 95, 70, 90, 70, poliwhirl.OwnerId ?? "Unknown", 60, "Water Absorb")
    {
        Id = poliwhirl.Id;
        Level = 1;
        Nickname = poliwhirl.Nickname;
        Experience = 0;
        HpIV = poliwhirl.HpIV;
        AttackIV = poliwhirl.AttackIV;
        SpecialAttackIV = poliwhirl.SpecialAttackIV;
        DefenseIV = poliwhirl.DefenseIV;
        SpecialDefenseIV = poliwhirl.SpecialDefenseIV;
        SpeedIV = poliwhirl.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;
        SkillPool = "Hypnosis, Water Gun, DoubleSlap, Body Slam, Amnesia, Hydro Pump, Submission, Counter, Seismic Toss, Strength, Earthquake, Toxic, Take Down, Double-Edge, Ice Beam, Blizzard, Hyper Beam, Mimic, Double Team, Reflect, Bide, Rest, Substitute, Surf";

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
