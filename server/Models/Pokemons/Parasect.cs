using Server;
using Database;
namespace PokemonPocket;

public class Parasect : PokemonMaster
{
    public override string? Requirements { get; set; } = "Unevolvable";
    private Parasect() { } //For EF Core
    public Parasect(string nickname, string ownerId) 
    : base("Parasect", "Bug/Grass", 60, 95, 80, 60, 80, 30, ownerId, 24, "Effect Spore")
    {
        Nickname = nickname;
        SkillPool = "Scratch, Stun Spore, Leech Life, Spore, Slash, Growth, SolarBeam, Toxic, Body Slam, Take Down, Double-Edge, Hyper Beam, Mimic, Double Team, Reflect, Bide, Rest, Substitute, Cut";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }
    
    public Parasect(Paras paras)
    : base("Parasect", "Bug/Grass", 60, 95, 80, 60, 80, 30, paras.OwnerId ?? "Unknown", 24, "Effect Spore")
    {
        Id = paras.Id;
        Level = 1;
        Nickname = paras.Nickname;
        Experience = paras.Experience;
        HpIV = paras.HpIV;
        AttackIV = paras.AttackIV;
        SpecialAttackIV = paras.SpecialAttackIV;
        DefenseIV = paras.DefenseIV;
        SpecialDefenseIV = paras.SpecialDefenseIV;
        SpeedIV = paras.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;
        SkillPool = "Scratch, Stun Spore, Leech Life, Spore, Slash, Growth, SolarBeam, Toxic, Body Slam, Take Down, Double-Edge, Hyper Beam, Mimic, Double Team, Reflect, Bide, Rest, Substitute, Cut";

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