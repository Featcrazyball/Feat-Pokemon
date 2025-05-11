using Server;
using Database;
namespace PokemonPocket;

public class Vileplume : PokemonMaster
{
    public override string? Requirements { get; set; } = "Unevolvable";
    private Vileplume() { } //For EF Core
    public Vileplume(string nickname, string ownerId) 
    : base("Vileplume", "Grass/Poison", 75, 80, 85, 110, 90, 50, ownerId, 20, "Effect Spore")
    {
        Nickname = nickname;
        SkillPool = "Absorb, Poison Powder, Stun Spore, Sleep Powder, Acid, Petal Dance, Solar Beam, Toxic, Body Slam, Take Down, Double-Edge, Hyper Beam, Rage, Mimic, Double Team, Reflect, Bide, Rest, Substitute, Cut";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Vileplume(Gloom gloom)
    : base("Vileplume", "Grass/Poison", 75, 80, 85, 110, 90, 50, gloom.OwnerId ?? "Unknown", 20, "Effect Spore")
    {
        Id = gloom.Id;
        Level = 1;
        Nickname = gloom.Nickname;
        Experience = gloom.Experience;
        HpIV = gloom.HpIV;
        AttackIV = gloom.AttackIV;
        SpecialAttackIV = gloom.SpecialAttackIV;
        DefenseIV = gloom.DefenseIV;
        SpecialDefenseIV = gloom.SpecialDefenseIV;
        SpeedIV = gloom.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;
        SkillPool = "Absorb, Poison Powder, Stun Spore, Sleep Powder, Acid, Petal Dance, Solar Beam, Toxic, Body Slam, Take Down, Double-Edge, Hyper Beam, Rage, Mimic, Double Team, Reflect, Bide, Rest, Substitute, Cut";

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
        await session.SendMessageAsync($"{Nickname} is already at its final evolution stage.");
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}