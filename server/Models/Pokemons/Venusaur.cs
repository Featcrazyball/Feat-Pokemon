using Server;
using Database;
namespace PokemonPocket;

public class Venusaur : PokemonMaster
{
    public override float HealthOverride {get;set;} = 80;
    public override string? Requirements { get; set; } = "Unevolvable";
    private Venusaur() { } //For EF Core
    public Venusaur(string nickname, string ownerId) 
    : base("Venusaur", "Grass/Poison", 80, 82, 83, 100, 100, 80, ownerId, 30, "Water Burst")
    {
        Nickname = nickname;
        SkillPool = "Tackle, Growl, Leech Seed, Vine Whip, Poison Powder, Razor Leaf, Growth, Sleep Powder, Solar Beam, Toxic, Body Slam, Take Down, Double-Edge, Hyper Beam, Rage, Mimic, Double Team, Reflect, Bide, Rest, Substitute, Cut";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Venusaur(float HP, string nickname, string ownerId, int exp)
    : base("Venusaur", "Grass/Poison", HP, 82, 83, 100, 100, 80, ownerId, 30, "Water Burst")
    {
        Nickname = nickname;
        Experience = exp;
        SkillPool = "Tackle, Growl, Leech Seed, Vine Whip, Poison Powder, Razor Leaf, Growth, Sleep Powder, Solar Beam, Toxic, Body Slam, Take Down, Double-Edge, Hyper Beam, Rage, Mimic, Double Team, Reflect, Bide, Rest, Substitute, Cut";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Venusaur(Ivysaur ivysaur)
    : base("Venusaur", "Grass/Poison", 100, 82, 83, 100, 100, 80, ivysaur.OwnerId ?? "Unknown", 30, "Water Burst")
    {
        Id = ivysaur.Id;
        Level = 1;
        Nickname = ivysaur.Nickname;
        Experience = 0;
        HpIV = ivysaur.HpIV;
        AttackIV = ivysaur.AttackIV;
        SpecialAttackIV = ivysaur.SpecialAttackIV;
        DefenseIV = ivysaur.DefenseIV;
        SpecialDefenseIV = ivysaur.SpecialDefenseIV;
        SpeedIV = ivysaur.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;
        SkillPool = "Tackle, Growl, Leech Seed, Vine Whip, Poison Powder, Razor Leaf, Growth, Sleep Powder, Solar Beam, Toxic, Body Slam, Take Down, Double-Edge, Hyper Beam, Rage, Mimic, Double Team, Reflect, Bide, Rest, Substitute, Cut";

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
        await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} is already at its final form!");
    }

    public override float calculateDamage(float SkillDamage) {
        return 2*SkillDamage;
    }
}
