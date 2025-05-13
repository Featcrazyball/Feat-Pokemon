using Server;
using Database;
namespace PokemonPocket;

public class Kingler : PokemonMaster
{
    public override string? Requirements { get; set; } = "Unevolvable";
    private Kingler() { } //For EF Core
    public Kingler(string nickname, string ownerId) 
    : base("Kingler", "Water", 55, 130, 115, 50, 50, 75, ownerId, 30, "Hyper Cutter")
    {
        Nickname = nickname;
        SkillPool = "Bubble, Leer, ViceGrip, Guillotine, Stomp, Crabhammer, Harden, Toxic, Body Slam, Take Down, Double-Edge, BubbleBeam, Ice Beam, Blizzard, Hyper Beam, Rage, Mimic, Double Team, Reflect, Bide, Rest, Substitute, Surf, Strength";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Kingler(Krabby krabby)
    : base("Kingler", "Water", 55, 130, 115, 50, 50, 75, krabby.OwnerId ?? "Unknown", 30, "Hyper Cutter")
    {
        Id = krabby.Id;
        Level = 1;
        Nickname = krabby.Nickname;
        Experience = krabby.Experience;
        HpIV = krabby.HpIV;
        AttackIV = krabby.AttackIV;
        SpecialAttackIV = krabby.SpecialAttackIV;
        DefenseIV = krabby.DefenseIV;
        SpecialDefenseIV = krabby.SpecialDefenseIV;
        SpeedIV = krabby.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;
        SkillPool = "Bubble, Leer, Guillotine, Stomp, Crabhammer, Harden, Toxic, Body Slam, Take Down, Double-Edge, BubbleBeam, Ice Beam, Blizzard, Hyper Beam, Rage, Mimic, Double Team, Reflect, Bide, Rest, Substitute, Surf, Strength";

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