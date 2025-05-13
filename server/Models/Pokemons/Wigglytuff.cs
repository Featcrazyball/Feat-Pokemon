using Server;
using Database;
namespace PokemonPocket;

public class Wigglytuff : PokemonMaster
{
    public override string? Requirements { get; set; } = "Unevolvable";
    private Wigglytuff() { } //For EF Core
    public Wigglytuff(string nickname, string ownerId) 
    : base("Wigglytuff", "Normal/Fairy", 140, 70, 45, 85, 50, 45, ownerId, 30, "Cute Charm")
    {
        Nickname = nickname;
        SkillPool = "Sing, Disable, Defense Curl, Double Slap, Rest, Body Slam, Take Down, Double-Edge, Hyper Beam, Seismic Toss, Thunderbolt, Thunder, Psychic, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Wigglytuff(Jigglypuff jigglypuff)
    : base("Wigglytuff", "Normal/Fairy", 140, 70, 45, 85, 50, 45, jigglypuff.OwnerId ?? "Unknown", 30, "Cute Charm")
    {
        Id = jigglypuff.Id;
        Level = 1;
        Nickname = jigglypuff.Nickname;
        Experience = jigglypuff.Experience;
        HpIV = jigglypuff.HpIV;
        AttackIV = jigglypuff.AttackIV;
        SpecialAttackIV = jigglypuff.SpecialAttackIV;
        DefenseIV = jigglypuff.DefenseIV;
        SpecialDefenseIV = jigglypuff.SpecialDefenseIV;
        SpeedIV = jigglypuff.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;
        SkillPool = "Sing, Disable, Defense Curl, Double Slap, Rest, Body Slam, Take Down, Double-Edge, Hyper Beam, Seismic Toss, Thunderbolt, Thunder, Psychic, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

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