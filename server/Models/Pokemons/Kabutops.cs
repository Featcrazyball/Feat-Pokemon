using Server;
using Database;
namespace PokemonPocket;

public class Kabutops : PokemonMaster
{
    public override string? Requirements { get; set; } = "Unevolvable";
    private Kabutops() { } //For EF Core
    public Kabutops(string nickname, string ownerId) 
    : base("Kabutops", "Rock/Water", 60, 115, 105, 65, 70, 80, ownerId, 40, "Swift Swim")
    {
        Nickname = nickname;
        SkillPool = "Scratch, Harden, Absorb, Slash, Leer, Hydro Pump, Toxic, Body Slam, Take Down, Double-Edge, Bubble Beam, Ice Beam, Blizzard, Hyper Beam, Rage, Mimic, Double Team, Reflect, Bide, Rest, Substitute, Surf";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Kabutops(Kabuto kabuto)
    : base("Kabutops", "Rock/Water", 60, 115, 105, 65, 70, 80, kabuto.OwnerId?? "Unknown", 40, "Swift Swim")
    {
        Id = kabuto.Id;
        Level = 1;
        Nickname = kabuto.Nickname;
        Experience = kabuto.Experience;
        HpIV = kabuto.HpIV;
        AttackIV = kabuto.AttackIV;
        SpecialAttackIV = kabuto.SpecialAttackIV;
        DefenseIV = kabuto.DefenseIV;
        SpecialDefenseIV = kabuto.SpecialDefenseIV;
        SpeedIV = kabuto.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;
        SkillPool = "Scratch, Harden, Absorb, Slash, Leer, Hydro Pump, Toxic, Body Slam, Take Down, Double-Edge, Bubble Beam, Ice Beam, Blizzard, Hyper Beam, Rage, Mimic, Double Team, Reflect, Bide, Rest, Substitute, Surf";

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
        await session.SendMessageAsync($"{Nickname == "None" ? Name : Nickname} is already at its final evolution stage.");
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}