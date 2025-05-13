using Server;
using Database;
namespace PokemonPocket;

public class Golduck : PokemonMaster
{
    public override string? Requirements { get; set; } = "Unevolvable";
    private Golduck() { } //For EF Core
    public Golduck(string nickname, string ownerId) 
    : base("Golduck", "Water", 80, 82, 78, 95, 80, 85, ownerId, 55, "Damp")
    {
        Nickname = nickname;
        SkillPool = "Scratch, Tail Whip, Disable, Confusion, Screech, Fury Swipes, Hydro Pump, Psychic, Toxic, Body Slam, Take Down, Double-Edge, Seismic Toss, Rage, Mimic, Double Team, Reflect, Bide, Rest, Substitute, Surf";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Golduck(Psyduck psyduck)
    : base("Venomoth", "Bug/Poison", 80, 82, 78, 95, 80, 85, psyduck.OwnerId ?? "Unknown", 55, "Damp")
    {
        Id = psyduck.Id;
        Level = 1;
        Nickname = psyduck.Nickname;
        Experience = psyduck.Experience;
        HpIV = psyduck.HpIV;
        AttackIV = psyduck.AttackIV;
        SpecialAttackIV = psyduck.SpecialAttackIV;
        DefenseIV = psyduck.DefenseIV;
        SpecialDefenseIV = psyduck.SpecialDefenseIV;
        SpeedIV = psyduck.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;
        SkillPool = "Scratch, Tail Whip, Disable, Confusion, Screech, Fury Swipes, Hydro Pump, Psychic, Toxic, Body Slam, Take Down, Double-Edge, Seismic Toss, Rage, Mimic, Double Team, Reflect, Bide, Rest, Substitute, Surf";

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