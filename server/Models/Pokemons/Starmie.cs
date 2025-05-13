using Server;
using Database;
namespace PokemonPocket;

public class Starmie : PokemonMaster
{
    public override string? Requirements { get; set; } = "Unevolvable";
    private Starmie() { } //For EF Core
    public Starmie(string nickname, string ownerId) 
    : base("Starmie", "Water/Psychic", 60, 75, 85, 100, 85, 115, ownerId, 30, "Illuminate")
    {
        Nickname = nickname;
        SkillPool = "Tackle, Harden, Recover, Swift, Minimize, Light Screen, Hydro Pump, Surf, Thunderbolt, Ice Beam, Blizzard, Psychic, Body Slam, Seismic Toss, Toxic, Mimic, Double Team, Reflect, Bide, Rest, Substitute, Flash";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Starmie(Staryu staryu)
    : base("Starmie", "Water/Psychic", 60, 75, 85, 100, 85, 115, staryu.OwnerId ?? "Unknown", 30, "Illuminate")
    {
        Id = staryu.Id;
        Level = 1;
        Nickname = staryu.Nickname;
        Experience = staryu.Experience;
        HpIV = staryu.HpIV;
        AttackIV = staryu.AttackIV;
        SpecialAttackIV = staryu.SpecialAttackIV;
        DefenseIV = staryu.DefenseIV;
        SpecialDefenseIV = staryu.SpecialDefenseIV;
        SpeedIV = staryu.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;
        SkillPool = "Tackle, Harden, Recover, Swift, Minimize, Light Screen, Hydro Pump, Surf, Thunderbolt, Ice Beam, Blizzard, Psychic, Body Slam, Seismic Toss, Toxic, Mimic, Double Team, Reflect, Bide, Rest, Substitute, Flash";

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