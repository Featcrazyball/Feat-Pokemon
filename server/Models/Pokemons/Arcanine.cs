using Server;
using Database;
namespace PokemonPocket;

public class Arcanine : PokemonMaster
{
    public override float HealthOverride {get;set;} = 90;
    public override string? Requirements { get; set; } = "Unevolvable";
    private Arcanine() { } //For EF Core
    public Arcanine(string nickname, string ownerId) 
    : base("Arcanine", "Fire", 90, 110, 80, 100, 80, 95, ownerId, 59, "Intimidate")
    {
        Nickname = nickname;
        SkillPool = "Ember, Leer, Take Down, Agility, Flamethrower, Toxic, Body Slam, Double-Edge, Rage, Dragon Rage, Dig, Mimic, Fire Blast, Swift, Skull Bash, Rest, Substitute";
        
        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Arcanine(Growlithe growlithe)
    : base("Arcanine", "Fire", 90, 110, 80, 100, 80, 95, growlithe.OwnerId ?? "Unknown", 59, "Intimidate")
    {
        Id = growlithe.Id;
        Level = 1;
        Nickname = growlithe.Nickname;
        Experience = growlithe.Experience;
        HpIV = growlithe.HpIV;
        AttackIV = growlithe.AttackIV;
        SpecialAttackIV = growlithe.SpecialAttackIV;
        DefenseIV = growlithe.DefenseIV;
        SpecialDefenseIV = growlithe.SpecialDefenseIV;
        SpeedIV = growlithe.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;
        SkillPool = "Ember, Leer, Take Down, Agility, Flamethrower, Toxic, Body Slam, Double-Edge, Rage, Dragon Rage, Dig, Mimic, Fire Blast, Swift, Skull Bash, Rest, Substitute";

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
