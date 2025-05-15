using Server;
using Database;
namespace PokemonPocket;

public class Victreebel : PokemonMaster
{
    public override float HealthOverride {get;set;} = 80;
    public override string? Requirements { get; set; } = "Unevolvable";
    private Victreebel() { } //For EF Core
    public Victreebel(string nickname, string ownerId) 
    : base("Victreebel", "Grass/Poison", 80, 105, 65, 100, 70, 70, ownerId, 20, "Chlorophyll")
    {
        Nickname = nickname;
        SkillPool = "Vine Whip, Sleep Powder, Stun Spore, Acid, Razor Leaf, Growth, Wrap, Poison Powder, Solar Beam, Toxic, Body Slam, Take Down, Double-Edge, Hyper Beam, Rage, Mimic, Double Team, Reflect, Bide, Rest, Substitute, Cut";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Victreebel(float HP, string nickname, string ownerId, int exp)
    : base("Victreebel", "Grass/Poison", HP, 105, 65, 100, 70, 70, ownerId, 20, "Chlorophyll")
    {
        Nickname = nickname;
        Experience = exp;
        SkillPool = "Vine Whip, Sleep Powder, Stun Spore, Acid, Razor Leaf, Growth, Wrap, Poison Powder, Solar Beam, Toxic, Body Slam, Take Down, Double-Edge, Hyper Beam, Rage, Mimic, Double Team, Reflect, Bide, Rest, Substitute, Cut";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Victreebel(Weepinbell weepinbell)
    : base("Victreebel", "Grass/Poison", 100, 105, 65, 100, 70, 70, weepinbell.OwnerId ?? "Unknown", 20, "Chlorophyll")
    {
        Id = weepinbell.Id;
        Level = 1;
        Nickname = weepinbell.Nickname;
        Experience = 0;
        HpIV = weepinbell.HpIV;
        AttackIV = weepinbell.AttackIV;
        SpecialAttackIV = weepinbell.SpecialAttackIV;
        DefenseIV = weepinbell.DefenseIV;
        SpecialDefenseIV = weepinbell.SpecialDefenseIV;
        SpeedIV = weepinbell.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;
        SkillPool = "Vine Whip, Sleep Powder, Stun Spore, Acid, Razor Leaf, Growth, Wrap, Poison Powder, Solar Beam, Toxic, Body Slam, Take Down, Double-Edge, Hyper Beam, Rage, Mimic, Double Team, Reflect, Bide, Rest, Substitute, Cut";

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
