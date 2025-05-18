using Server;
using Database;

namespace PokemonPocket;

public class Jolteon : PokemonMaster
{
    public override float HealthOverride {get;set;} = 65;
    public override string? Requirements { get; set; } = "Unevolvable";
    private Jolteon() { } //For EF Core
    public Jolteon(string nickname, string ownerId) 
    : base("Jolteon", "Electric", 65, 65, 60, 110, 95, 130, ownerId, 29, "Volt Absorb")
    {
        Nickname = nickname;
        SkillPool = "Tackle, Sand Attack, Growl, Quick Attack, Thunder Shock, Thunder Wave, Double Kick, Agility, Pin Missile, Thunder, Body Slam, Take Down, Double-Edge, Rage, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Jolteon(float HP, string nickname, string ownerId, int exp)
    : base("Jolteon", "Electric", HP, 65, 60, 110, 95, 130, ownerId, 29, "Volt Absorb")
    {
        Nickname = nickname;
        Experience = exp;
        SkillPool = "Tackle, Sand Attack, Growl, Quick Attack, Thunder Shock, Thunder Wave, Double Kick, Agility, Pin Missile, Thunder, Body Slam, Take Down, Double-Edge, Rage, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Jolteon(Eevee eevee)
    : base("Jolteon", "Electric", 100, 65, 60, 110, 95, 130, eevee.OwnerId?? "Unknown", 29, "Volt Absorb")
    {
        Id = eevee.Id;
        Level = 1;
        Nickname = eevee.Nickname;
        Experience = 0;
        HpIV = eevee.HpIV;
        AttackIV = eevee.AttackIV;
        SpecialAttackIV = eevee.SpecialAttackIV;
        DefenseIV = eevee.DefenseIV;
        SpecialDefenseIV = eevee.SpecialDefenseIV;
        SpeedIV = eevee.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;
        SkillPool = "Tackle, Sand Attack, Growl, Quick Attack, Thunder Shock, Thunder Wave, Double Kick, Agility, Pin Missile, Thunder, Body Slam, Take Down, Double-Edge, Rage, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public override async Task GodEvolve(ClientSession session)
    {
        await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} is already at its final evolution stage.");
    }

    public override async Task Evolve(ClientSession session)
    {
        await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} is already at its final evolution stage.");
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}
