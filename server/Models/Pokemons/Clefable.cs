using Server;
using Database;
namespace PokemonPocket;

public class Clefable : PokemonMaster
{
    public override string? Requirements { get; set; } = "Unevolvable";

    private Clefable() { } //For EF Core
    public Clefable(string nickname, string ownerId) 
    : base("Clefable", "Fairy", 95, 70, 73, 95, 90, 60, ownerId, 35, "Cute Charm")
    {
        Nickname = nickname;
        SkillPool = "Sing, Double Slap, Minimize, Metronome, Defense Curl, Light Screen, Solar Beam, Thunderbolt, Thunder, Psychic, Teleport, Seismic Toss, Counter, Toxic, Body Slam, Take Down, Double-Edge, Submission, Rage, Dig, Mimic, Double Team, Reflect, Bide, Fire Blast, Swift, Skull Bash, Rest, Psywave, Substitute";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Clefable(Clefairy clefairy)
    : base("Clefable", "Fairy", 95, 70, 73, 95, 90, 60, clefairy.OwnerId ?? "Unknown", 35, "Cute Charm")
    {
        Id = clefairy.Id;
        Level = 1;
        Nickname = clefairy.Nickname;
        Experience = clefairy.Experience;
        HpIV = clefairy.HpIV;
        AttackIV = clefairy.AttackIV;
        SpecialAttackIV = clefairy.SpecialAttackIV;
        DefenseIV = clefairy.DefenseIV;
        SpecialDefenseIV = clefairy.SpecialDefenseIV;
        SpeedIV = clefairy.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;
        SkillPool = "Sing, Double Slap, Minimize, Metronome, Defense Curl, Light Screen, Solar Beam, Thunderbolt, Thunder, Psychic, Teleport, Seismic Toss, Counter, Toxic, Body Slam, Take Down, Double-Edge, Submission, Rage, Dig, Mimic, Double Team, Reflect, Bide, Fire Blast, Swift, Skull Bash, Rest, Psywave, Substitute";

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
        await session.SendMessageAsync($"{Nickname == "None" ? Name : Nickname} is already at its final evolution stage.");
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}