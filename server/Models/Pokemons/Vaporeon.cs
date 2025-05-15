using Server;
using Database;
namespace PokemonPocket;

public class Vaporeon : PokemonMaster
{
    public override float HealthOverride {get;set;} = 130;
    public override string? Requirements { get; set; } = "Unevolvable";
    private Vaporeon() { } //For EF Core
    public Vaporeon(string nickname, string ownerId) 
    : base("Vaporeon", "Water", 130, 65, 60, 110, 95, 65, ownerId, 30, "Water Absorb")
    {
        Nickname = nickname;
        SkillPool = "Tackle, Sand Attack, Quick Attack, Water Gun, Tail Whip, Bite, Aurora Beam, Haze, Mist, Acid Armor, Hydro Pump, Toxic, Body Slam, Take Down, Double-Edge, Bubble Beam, Ice Beam, Blizzard, Mimic, Double Team, Reflect, Bide, Rest, Substitute, Surf";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Vaporeon(Eevee eevee)
    : base("Vaporeon", "Water", 130, 65, 60, 110, 95, 65, eevee.OwnerId?? "Unknown", 30, "Water Absorb")
    {
        Id = eevee.Id;
        Level = 1;
        Nickname = eevee.Nickname;
        Experience = eevee.Experience;
        HpIV = eevee.HpIV;
        AttackIV = eevee.AttackIV;
        SpecialAttackIV = eevee.SpecialAttackIV;
        DefenseIV = eevee.DefenseIV;
        SpecialDefenseIV = eevee.SpecialDefenseIV;
        SpeedIV = eevee.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;
        SkillPool = "Tackle, Sand Attack, Quick Attack, Water Gun, Tail Whip, Bite, Aurora Beam, Haze, Mist, Acid Armor, Hydro Pump, Toxic, Body Slam, Take Down, Double-Edge, BubbleBeam, Ice Beam, Blizzard, Mimic, Double Team, Reflect, Bide, Rest, Substitute, Surf";

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
