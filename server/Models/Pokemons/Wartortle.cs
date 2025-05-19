using Database;
using Server;
namespace PokemonPocket;

public class Wartortle : PokemonMaster
{
    public override float HealthOverride {get;set;} = 59;
    public override string? Requirements { get; set; } = "Level 36";
    public override string? EvolvesTo {get;set;} = "Blastoise";
    private Wartortle() { } //For EF Core
    public Wartortle(string nickname, string ownerId) 
    : base("Wartortle", "Water", 59, 63, 80, 65, 80, 58, ownerId, 25, "Water Gun")
    {
        Nickname = nickname;
        SkillPool = "Tackle, Tail Whip, Bubble, Water Gun, Bite, Withdraw, Skull Bash, Hydro Pump, Surf, Ice Beam, Blizzard, Body Slam, Seismic Toss, Toxic, Mimic, Double Team, Reflect, Bide, Rest, Substitute, Strength";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Wartortle(float HP, string nickname, string ownerId, int exp)
    : base("Wartortle", "Water", HP, 63, 80, 65, 80, 58, ownerId, 25, "Water Gun")
    {
        Nickname = nickname;
        Experience = exp;
        SkillPool = "Tackle, Tail Whip, Bubble, Water Gun, Bite, Withdraw, Skull Bash, Hydro Pump, Surf, Ice Beam, Blizzard, Body Slam, Seismic Toss, Toxic, Mimic, Double Team, Reflect, Bide, Rest, Substitute, Strength";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Wartortle(Squirtle squirtle)
    : base("Wartortle", "Water", 100, 63, 80, 65, 80, 58, squirtle.OwnerId ?? "Unknown", 25, "Water Gun")
    {
        Id = squirtle.Id;
        Level = 1;
        Nickname = squirtle.Nickname;
        Experience = 0;
        HpIV = squirtle.HpIV;
        AttackIV = squirtle.AttackIV;
        SpecialAttackIV = squirtle.SpecialAttackIV;
        DefenseIV = squirtle.DefenseIV;
        SpecialDefenseIV = squirtle.SpecialDefenseIV;
        SpeedIV = squirtle.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;
        SkillPool = "Tackle, Tail Whip, Bubble, Water Gun, Bite, Withdraw, Skull Bash, Hydro Pump, Surf, Ice Beam, Blizzard, Body Slam, Seismic Toss, Toxic, Mimic, Double Team, Reflect, Bide, Rest, Substitute, Strength";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }
    
    public Wartortle(string ownerId)
    : base("Wartortle", "Water", 100, 63, 80, 65, 80, 58, ownerId, 25, "Water Gun")
    {
        Nickname = "None";
        Experience = 0;
        SkillPool = "Tackle, Tail Whip, Bubble, Water Gun, Bite, Withdraw, Skull Bash, Hydro Pump, Surf, Ice Beam, Blizzard, Body Slam, Seismic Toss, Toxic, Mimic, Double Team, Reflect, Bide, Rest, Substitute, Strength";

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
        if (Level >= 36)
        {
            using (var context = new DatabaseContext())
            {
                var blastoise = new Blastoise(this);
                blastoise.MaxHealth = blastoise.HealthOverride;
                blastoise.EvolveLevelUp(Level - 1); // Level up to 36

                foreach (var skill in this.Skills)
                {
                    context.Skills.Remove(skill);
                }

                context.PokemonMaster.Remove(this);
                context.PokemonMaster.Add(blastoise);
                foreach (var skill in blastoise.Skills)
                {
                    context.Skills.Add(skill);
                }

                // Remove previous and add new Pokemon
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} has evolved from a Wartortle to a Blastoise!");
        }
        else
        {
            await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} is not ready to evolve yet.");
        }
    }

    public override async Task GodEvolve(ClientSession session)
    {
        using (var context = new DatabaseContext())
        {
            var blastoise = new Blastoise(this);
            blastoise.MaxHealth = blastoise.HealthOverride;
            blastoise.EvolveLevelUp(Level-1); // Level up to 36

            foreach (var skill in this.Skills)
            {
                context.Skills.Remove(skill);
            }

            context.PokemonMaster.Remove(this);
            context.PokemonMaster.Add(blastoise);
            foreach (var skill in blastoise.Skills)
            {
                context.Skills.Add(skill);
            }

            // Remove previous and add new Pokemon
            context.SaveChanges();
        }
        await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} has evolved from a Wartortle to a Blastoise!");
    }

    public override float calculateDamage(float SkillDamage)
    {
        return SkillDamage;
    }
}
