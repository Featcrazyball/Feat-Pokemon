using Server;
using Database;
namespace PokemonPocket;

public class Arbok : PokemonMaster
{
    public override float HealthOverride {get;set;} = 60;
    public override string? Requirements { get; set; } = "Unevolvable";
    private Arbok() { } //For EF Core
    public Arbok(string nickname, string ownerId) 
    : base("Arbok", "Poison", 60, 95, 69, 65, 79, 80, ownerId, 25, "Bite")
    {
        Nickname = nickname;
        SkillPool = "Wrap, Poison Sting, Bite, Glare, Acid, Screech, Toxic, Body Slam, Take Down, Double-Edge, Rage, Earthquake, Fissure, Skull Bash, Rock Slide, Rest, Substitute";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Arbok(Ekans ekans)
    : base("Arbok", "Poison", 60, 95, 69, 65, 79, 80, ekans.OwnerId ?? "Unknown", 25, "Bite")
    {
        Id = ekans.Id;
        Level = 1;
        Nickname = ekans.Nickname;
        Experience = ekans.Experience;
        HpIV = ekans.HpIV;
        AttackIV = ekans.AttackIV;
        SpecialAttackIV = ekans.SpecialAttackIV;
        DefenseIV = ekans.DefenseIV;
        SpecialDefenseIV = ekans.SpecialDefenseIV;
        SpeedIV = ekans.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;
        
        SkillPool = "Wrap, Poison Sting, Bite, Glare, Acid, Screech, Toxic, Body Slam, Take Down, Double-Edge, Rage, Earthquake, Fissure, Skull Bash, Rock Slide, Rest, Substitute";
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
