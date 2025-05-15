using Database;
using Server;
namespace PokemonPocket;

public class Dragonair : PokemonMaster
{
    public override float HealthOverride {get;set;} = 61;
    public override string? Requirements { get; set; } = "Level 55";
    public override string? EvolvesTo {get;set;} = "Dragonite";
    private Dragonair() { } //For EF Core
    public Dragonair(string nickname, string ownerId) 
    : base("Dragonair", "Dragon", 61, 84, 65, 70, 70, 70, ownerId, 30, "Shed Skin")
    {
        Nickname = nickname;
        SkillPool = "Wrap, Leer, Thunder Wave, Agility, Slam, Dragon Rage, Hyper Beam, Toxic, Body Slam, Take Down, Double-Edge, Blizzard, Rage, Thunderbolt, Thunder, Surf, Mimic, Double Team, Reflect, Bide, Fire Blast, Swift, Skull Bash, Rest, Substitute";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Dragonair(Dratini dratini)
    : base("Dragonair", "Dragon", 61, 84, 65, 70, 70, 70, dratini.OwnerId?? "Unknown", 30, "Shed Skin")
    {
        Id = dratini.Id;
        Level = 1;
        Nickname = dratini.Nickname;
        Experience = dratini.Experience;
        HpIV = dratini.HpIV;
        AttackIV = dratini.AttackIV;
        SpecialAttackIV = dratini.SpecialAttackIV;
        DefenseIV = dratini.DefenseIV;
        SpecialDefenseIV = dratini.SpecialDefenseIV;
        SpeedIV = dratini.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;
        SkillPool = "Wrap, Leer, Thunder Wave, Agility, Slam, Dragon Rage, Hyper Beam, Toxic, Body Slam, Take Down, Double-Edge, Blizzard, Rage, Thunderbolt, Thunder, Surf, Mimic, Double Team, Reflect, Bide, Fire Blast, Swift, Skull Bash, Rest, Substitute";

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
        if (Level >= 55) {
            using (var context = new DatabaseContext())
            {
                var dragonite = new Dragonite(this);
                dragonite.EvolveLevelUp(Level-1);

                foreach (var skill in this.Skills)
                {
                    context.Skills.Remove(skill);
                }

                // Add skills for the evolved Pokemon
                context.PokemonMaster.Remove(this);
                context.PokemonMaster.Add(dragonite);
                foreach (var skill in dragonite.Skills)
                {
                    context.Skills.Add(skill);
                }
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} has evolved from a Dragonair to a Dragonite!");
        } else {
            await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}
