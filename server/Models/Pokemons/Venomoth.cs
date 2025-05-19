using Server;
using Database;
namespace PokemonPocket;

public class Venomoth : PokemonMaster
{
    public override float HealthOverride {get;set;} = 70;
    public override string? Requirements { get; set; } = "Unevolvable";
    private Venomoth() { } //For EF Core
    public Venomoth(string nickname, string ownerId) 
    : base("Venomoth", "Bug/Poison", 70, 65, 60, 90, 75, 90, ownerId, 31, "Shield Dust")
    {
        Nickname = nickname;
        SkillPool = "Tackle, Disable, Supersonic, Confusion, Poison Powder, Leech Life, Stun Spore, Psybeam, Sleep Powder, Toxic, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Venomoth(float HP, string nickname, string ownerId, int exp)
    : base("Venomoth", "Bug/Poison", HP, 65, 60, 90, 75, 90, ownerId, 31, "Shield Dust")
    {
        Nickname = nickname;
        Experience = exp;
        SkillPool = "Tackle, Disable, Supersonic, Confusion, Poison Powder, Leech Life, Stun Spore, Psybeam, Sleep Powder, Toxic, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Venomoth(Venonat venonat)
    : base("Venomoth", "Bug/Poison", 100, 65, 60, 90, 75, 90, venonat.OwnerId ?? "Unknown", 31, "Shield Dust")
    {
        Id = venonat.Id;
        Level = 1;
        Nickname = venonat.Nickname;
        Experience = 0;
        HpIV = venonat.HpIV;
        AttackIV = venonat.AttackIV;
        SpecialAttackIV = venonat.SpecialAttackIV;
        DefenseIV = venonat.DefenseIV;
        SpecialDefenseIV = venonat.SpecialDefenseIV;
        SpeedIV = venonat.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;
        SkillPool = "Tackle, Disable, Supersonic, Confusion, Poison Powder, Leech Life, Stun Spore, Psybeam, Sleep Powder, Toxic, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Venomoth(string ownerId)
    : base("Venomoth", "Bug/Poison", 100, 65, 60, 90, 75, 90, ownerId, 31, "Shield Dust")
    {
        Nickname = "None";
        SkillPool = "Tackle, Disable, Supersonic, Confusion, Poison Powder, Leech Life, Stun Spore, Psybeam, Sleep Powder, Toxic, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

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
