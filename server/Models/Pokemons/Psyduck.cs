using Database;
using Server;
namespace PokemonPocket;

public class Psyduck : PokemonMaster
{
    public override float HealthOverride {get;set;} = 50;
    public override string? Requirements { get; set; } = "Level 33";
    public override string? EvolvesTo {get;set;} = "Golduck";
    private Psyduck() { } //For EF Core
    public Psyduck(string nickname, string ownerId) 
    : base("Psyduck", "Water", 50, 52, 48, 65, 50, 55, ownerId, 33, "Damp")
    {
        Nickname = nickname;
        SkillPool = "Scratch, Tail Whip, Disable, Confusion, Fury Swipes, Hydro Pump, Seismic Toss, Counter, Strength, Surf, Toxic, Body Slam, Take Down, Double-Edge, Ice Beam, Blizzard, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Psyduck(float HP, string nickname, string ownerId, int exp)
    : base("Psyduck", "Water", HP, 52, 48, 65, 50, 55, ownerId, 33, "Damp")
    {
        Nickname = nickname;
        Experience = exp;
        SkillPool = "Scratch, Tail Whip, Disable, Confusion, Fury Swipes, Hydro Pump, Seismic Toss, Counter, Strength, Surf, Toxic, Body Slam, Take Down, Double-Edge, Ice Beam, Blizzard, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

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
        if (Level >= 33) {
            using (var context = new DatabaseContext())
            {
                var golduck = new Golduck(this);
                golduck.MaxHealth = golduck.HealthOverride;
                golduck.EvolveLevelUp(Level-1);

                foreach (var skill in this.Skills)
                {
                    context.Skills.Remove(skill);
                }

                context.PokemonMaster.Remove(this);
                context.PokemonMaster.Add(golduck);
                foreach (var skill in golduck.Skills)
                {
                    context.Skills.Add(skill);
                }

                // Remove previous and add new Pokemon
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} has evolved from a Psyduck to a Golduck!");
        } else {
            await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}
