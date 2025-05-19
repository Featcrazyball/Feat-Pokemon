using Server;
using Database;
namespace PokemonPocket;

public class Charizard : PokemonMaster
{
    public override float HealthOverride {get;set;} = 78;
    public override string? Requirements { get; set; } = "Unevolvable";
    private Charizard() { } //For EF Core
    public Charizard(string nickname, string ownerId) 
    : base("Charizard", "Fire/Flying", 78, 84, 78, 109, 85, 100, ownerId, 40, "Fire Burst")
    {
        Nickname = nickname;
        SkillPool = "Scratch, Growl, Ember, Leer, Rage, Slash, Flamethrower, Fire Spin, Toxic, Body Slam, Take Down, Double-Edge, Hyper Beam, Submission, Seismic Toss, Counter, Dragon Rage, Earthquake, Fissure, Dig, Mimic, Double Team, Reflect, Bide, Fire Blast, Swift, Skull Bash, Rest, Substitute";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }
    
    public Charizard(string ownerId)
    : base("Charizard", "Fire/Flying", 100, 84, 78, 109, 85, 100, ownerId, 40, "Fire Burst")
    {
        Nickname = "None";
        Experience = 0;
        SkillPool = "Scratch, Growl, Ember, Leer, Rage, Slash, Flamethrower, Fire Spin, Toxic, Body Slam, Take Down, Double-Edge, Hyper Beam, Submission, Seismic Toss, Counter, Dragon Rage, Earthquake, Fissure, Dig, Mimic, Double Team, Reflect, Bide, Fire Blast, Swift, Skull Bash, Rest, Substitute";
        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Charizard(float HP, string nickname, string ownerId, int exp)
    : base("Charizard", "Fire/Flying", HP, 84, 78, 109, 85, 100, ownerId, 40, "Fire Burst")
    {
        Nickname = nickname;
        Experience = exp;
        SkillPool = "Scratch, Growl, Ember, Leer, Rage, Slash, Flamethrower, Fire Spin, Toxic, Body Slam, Take Down, Double-Edge, Hyper Beam, Submission, Seismic Toss, Counter, Dragon Rage, Earthquake, Fissure, Dig, Mimic, Double Team, Reflect, Bide, Fire Blast, Swift, Skull Bash, Rest, Substitute";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills)
            {
                Skills.Add(skill);
            }
            ;
        }
    }

    public Charizard(Charmeleon charmander)
    : base("Charizard", "Fire/Flying", 100, 84, 78, 109, 85, 100, charmander.OwnerId ?? "Unknown", 40, "Fire Burst")
    {
        Id = charmander.Id;
        Level = 1;
        Nickname = charmander.Nickname;
        Experience = 0;
        HpIV = charmander.HpIV;
        AttackIV = charmander.AttackIV;
        SpecialAttackIV = charmander.SpecialAttackIV;
        DefenseIV = charmander.DefenseIV;
        SpecialDefenseIV = charmander.SpecialDefenseIV;
        SpeedIV = charmander.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;
        SkillPool = "Scratch, Growl, Ember, Leer, Rage, Slash, Flamethrower, Fire Spin, Toxic, Body Slam, Take Down, Double-Edge, Hyper Beam, Submission, Seismic Toss, Counter, Dragon Rage, Earthquake, Fissure, Dig, Mimic, Double Team, Reflect, Bide, Fire Blast, Swift, Skull Bash, Rest, Substitute";

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
        await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} is already at its final form!");
    }
    
    public override async Task GodEvolve(ClientSession session)
    {
        await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} is already at its final evolution stage.");
    }

    public override float calculateDamage(float SkillDamage)
    {
        return SkillDamage;
    }
}
