using Database;
using Server;
namespace PokemonPocket;

public class Doduo : PokemonMaster
{
    public override float HealthOverride {get;set;} = 35;
    public override string? Requirements { get; set; } = "Level 30";
    public override string? EvolvesTo {get;set;} = "Dodrio";
    private Doduo() { } //For EF Core
    public Doduo(string nickname, string ownerId) 
    : base("Doduo", "Normal/Flying", 35, 85, 45, 35, 35, 75, ownerId, 20, "Run Away")
    {
        Nickname = nickname;
        SkillPool = "Peck, Growl, Fury Attack, Drill Peck, Rage, Agility, Tri Attack, Toxic, Body Slam, Take Down, Double-Edge, Rage, Mimic, Double Team, Reflect, Bide, Rest, Substitute, Fly";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Doduo(Doduo doduo)
    : base("Doduo", "Normal/Flying", 35, 85, 45, 35, 35, 75, doduo.OwnerId ?? "Unknown", 20, "Run Away")
    {
        Id = doduo.Id;
        Level = 1;
        Nickname = doduo.Nickname;
        Experience = doduo.Experience;
        HpIV = doduo.HpIV;
        AttackIV = doduo.AttackIV;
        SpecialAttackIV = doduo.SpecialAttackIV;
        DefenseIV = doduo.DefenseIV;
        SpecialDefenseIV = doduo.SpecialDefenseIV;
        SpeedIV = doduo.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;
        SkillPool = "Peck, Growl, Fury Attack, Drill Peck, Rage, Agility, Tri Attack, Toxic, Body Slam, Take Down, Double-Edge, Rage, Mimic, Double Team, Reflect, Bide, Rest, Substitute, Fly";

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
        if (Level >= 30) {
            using (var context = new DatabaseContext())
            {
                var dodrio = new Dodrio(this);
                dodrio.EvolveLevelUp(Level-1);
                foreach (var skill in this.Skills)
                {
                    context.Skills.Remove(skill);
                }
                // Add skills for the evolved Pokemon
                context.PokemonMaster.Remove(this);
                context.PokemonMaster.Add(dodrio);
                foreach (var skill in dodrio.Skills)
                {
                    context.Skills.Add(skill);
                }
                
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} has evolved from a Doduo to a Dodrio!");
        } else {
            await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}
