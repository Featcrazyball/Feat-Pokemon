using Server;
using Database;
namespace PokemonPocket;

public class Butterfree : PokemonMaster
{
    public override float HealthOverride {get;set;} = 60;
    public override string? Requirements { get; set; } = "Unevolvable";
    private Butterfree() { } //For EF Core
    public Butterfree(string nickname, string ownerId) 
    : base("Butterfree", "Bug/Flying", 60, 45, 50, 90, 80, 70, ownerId, 25, "Confusion")
    {
        Nickname = nickname;
        SkillPool = "Confusion, Poison Powder, Stun Spore, Sleep Powder, Psybeam, Supersonic, Whirlwind, Toxic, Psychic, Rage, Mimic, Double Team, Reflect, Bide, Swift, Rest, Substitute";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Butterfree(Metapod caterpie)
    : base("Butterfree", "Bug/Flying", 60, 45, 50, 90, 80, 70, caterpie.OwnerId ?? "Unknown", 25, "Confusion")
    {
        Id = caterpie.Id;
        Level = 1;
        Nickname = caterpie.Nickname;
        Experience = caterpie.Experience;
        HpIV = caterpie.HpIV;
        AttackIV = caterpie.AttackIV;
        SpecialAttackIV = caterpie.SpecialAttackIV;
        DefenseIV = caterpie.DefenseIV;
        SpecialDefenseIV = caterpie.SpecialDefenseIV;
        SpeedIV = caterpie.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;
        SkillPool = "Confusion, Poison Powder, Stun Spore, Sleep Powder, Psybeam, Supersonic, Whirlwind, Toxic, Psychic, Rage, Mimic, Double Team, Reflect, Bide, Swift, Rest, Substitute";

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

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}
