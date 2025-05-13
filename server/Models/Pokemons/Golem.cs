using Server;
using Database;
namespace PokemonPocket;

public class Golem : PokemonMaster
{
    public override string? Requirements { get; set; } = "Unevolvable";
    private Golem() { } //For EF Core
    public Golem(string nickname, string ownerId) 
    : base("Golem", "Rock/Ground", 80, 120, 130, 55, 65, 45, ownerId, 43, "Sturdy")
    {
        Nickname = nickname;
        SkillPool = "Tackle, Defense Curl, Rock Throw, Self-Destruct, Harden, Earthquake, Explosion, Toxic, Body Slam, Take Down, Double-Edge, Seismic Toss, Rage, Mimic, Double Team, Reflect, Bide, Fire Blast, Rest, Substitute, Strength";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Golem(Graveler graveler)
    : base("Golem", "Rock/Ground", 80, 120, 130, 55, 65, 45, graveler.OwnerId ?? "Unknown", 43, "Sturdy")
    {
        Id = graveler.Id;
        Level = 1;
        Nickname = graveler.Nickname;
        Experience = graveler.Experience;
        HpIV = graveler.HpIV;
        AttackIV = graveler.AttackIV;
        SpecialAttackIV = graveler.SpecialAttackIV;
        DefenseIV = graveler.DefenseIV;
        SpecialDefenseIV = graveler.SpecialDefenseIV;
        SpeedIV = graveler.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;
        SkillPool = "Tackle, Defense Curl, Rock Throw, Self-Destruct, Harden, Earthquake, Explosion, Toxic, Body Slam, Take Down, Double-Edge, Seismic Toss, Rage, Mimic, Double Team, Reflect, Bide, Fire Blast, Rest, Substitute, Strength";


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