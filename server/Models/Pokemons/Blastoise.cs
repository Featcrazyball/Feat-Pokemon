using Server;
using Database;
namespace PokemonPocket;

public class Blastoise : PokemonMaster
{
    public override float HealthOverride { get; set; } = 79;
    public override string? Requirements { get; set; } = "Unevolvable";
    private Blastoise() { } //For EF Core
    public Blastoise(string nickname, string ownerId)
    : base("Blastoise", "Water", 79, 83, 100, 85, 105, 78, ownerId, 30, "Torrent")
    {
        Nickname = nickname;
        SkillPool = "Tackle, Bubble, Water Gun, Bite, Withdraw, Skull Bash, Hydro Pump, Toxic, Body Slam, Take Down, Blizzard, Hyper Beam, Submission, Seismic Toss, Counter, Ice Beam, Dig, Mimic, Double Team, Bide, Rest, Substitute, Surf, Strength";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills)
            {
                Skills.Add(skill);
            }
        }
    }

    public Blastoise(float HP, string nickname, string ownerId, int exp)
    : base("Blastoise", "Water", HP, 83, 100, 85, 105, 78, ownerId, 30, "Torrent")
    {
        Nickname = nickname;
        Experience = exp;
        SkillPool = "Tackle, Bubble, Water Gun, Bite, Withdraw, Skull Bash, Hydro Pump, Toxic, Body Slam, Take Down, Blizzard, Hyper Beam, Submission, Seismic Toss, Counter, Ice Beam, Dig, Mimic, Double Team, Bide, Rest, Substitute, Surf, Strength";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills)
            {
                Skills.Add(skill);
            }
        }
    }

    public Blastoise(Wartortle wartortle)
    : base("Blastoise", "Water", 100, 83, 100, 85, 105, 78, wartortle.OwnerId ?? "Unknown", 30, "Torrent")
    {
        Id = wartortle.Id;
        Level = 1;
        Nickname = wartortle.Nickname;
        Experience = 0;
        HpIV = wartortle.HpIV;
        AttackIV = wartortle.AttackIV;
        SpecialAttackIV = wartortle.SpecialAttackIV;
        DefenseIV = wartortle.DefenseIV;
        SpecialDefenseIV = wartortle.SpecialDefenseIV;
        SpeedIV = wartortle.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;
        SkillPool = "Tackle, Bubble, Water Gun, Bite, Withdraw, Skull Bash, Hydro Pump, Toxic, Body Slam, Take Down, Blizzard, Hyper Beam, Submission, Seismic Toss, Counter, Ice Beam, Dig, Mimic, Double Team, Bide, Rest, Substitute, Surf, Strength";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills)
            {
                Skills.Add(skill);
            }
        }
    }

    public override async Task Evolve(ClientSession session)
    {
        await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} is already at its final evolution stage.");
    }

    public override float calculateDamage(float SkillDamage)
    {
        return 2 * SkillDamage;
    }

    public override async Task GodEvolve(ClientSession session)
    {
        await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} is already at its final evolution stage.");
    }
}