using Server;
using Database;
namespace PokemonPocket;

public class Slowbro : PokemonMaster
{
    public override float HealthOverride {get;set;} = 95;
    public override string? Requirements { get; set; } = "Unevolvable";
    private Slowbro() { } //For EF Core
    public Slowbro(string nickname, string ownerId) 
    : base("Slowbro", "Water/Psychic", 95, 75, 110, 100, 80, 30, ownerId, 36, "Oblivious")
    {
        Nickname = nickname;
        SkillPool = "Confusion, Disable, Headbutt, Growl, Water Gun, Withdraw, Amnesia, Psychic, Surf, Ice Beam, Blizzard, Body Slam, Seismic Toss, Toxic, Mimic, Double Team, Reflect, Bide, Rest, Substitute, Strength";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Slowbro(float HP, string nickname, string ownerId, int exp)
    : base("Slowbro", "Water/Psychic", HP, 75, 110, 100, 80, 30, ownerId, 36, "Oblivious")
    {
        Nickname = nickname;
        Experience = exp;
        SkillPool = "Confusion, Disable, Headbutt, Growl, Water Gun, Withdraw, Amnesia, Psychic, Surf, Ice Beam, Blizzard, Body Slam, Seismic Toss, Toxic, Mimic, Double Team, Reflect, Bide, Rest, Substitute, Strength";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Slowbro(Slowpoke slowpoke)
    : base("Slowbro", "Water/Psychic", 100, 75, 110, 100, 80, 30, slowpoke.OwnerId ?? "Unknown", 36, "Oblivious")
    {
        Id = slowpoke.Id;
        Level = 1;
        Nickname = slowpoke.Nickname;
        Experience = 0;
        HpIV = slowpoke.HpIV;
        AttackIV = slowpoke.AttackIV;
        SpecialAttackIV = slowpoke.SpecialAttackIV;
        DefenseIV = slowpoke.DefenseIV;
        SpecialDefenseIV = slowpoke.SpecialDefenseIV;
        SpeedIV = slowpoke.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;
        SkillPool = "Confusion, Disable, Headbutt, Growl, Water Gun, Withdraw, Amnesia, Psychic, Surf, Ice Beam, Blizzard, Body Slam, Seismic Toss, Toxic, Mimic, Double Team, Reflect, Bide, Rest, Substitute, Strength";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Slowbro(string ownerId)
    : base("Slowbro", "Water/Psychic", 100, 75, 110, 100, 80, 30, ownerId, 36, "Oblivious")
    {
        Nickname = "None";
        Experience = 0;
        SkillPool = "Confusion, Disable, Headbutt, Growl, Water Gun, Withdraw, Amnesia, Psychic, Surf, Ice Beam, Blizzard, Body Slam, Seismic Toss, Toxic, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

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
