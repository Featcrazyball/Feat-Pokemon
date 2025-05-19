using Database;
using Server;
namespace PokemonPocket;

public class Weepinbell : PokemonMaster
{
    public override float HealthOverride {get;set;} = 65;
    public override string? Requirements { get; set; } = "1 Leaf Stone";
    public override string? EvolvesTo {get;set;} = "Victreebel";
    private Weepinbell() { } //For EF Core
    public Weepinbell(string nickname, string ownerId) 
    : base("Weepinbell", "Grass/Poison", 65, 90, 50, 85, 45, 55, ownerId, 21, "Chlorophyll")
    {
        Nickname = nickname;
        SkillPool = "Vine Whip, Sleep Powder, Stun Spore, Acid, Razor Leaf, Growth, Wrap, PoisonPowder, SolarBeam, Toxic, Body Slam, Take Down, Double-Edge, Mimic, Double Team, Reflect, Bide, Rest, Substitute, Cut";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Weepinbell(float HP, string nickname, string ownerId, int exp)
    : base("Weepinbell", "Grass/Poison", HP, 90, 50, 85, 45, 55, ownerId, 21, "Chlorophyll")
    {
        Nickname = nickname;
        Experience = exp;
        SkillPool = "Vine Whip, Sleep Powder, Stun Spore, Acid, Razor Leaf, Growth, Wrap, PoisonPowder, SolarBeam, Toxic, Body Slam, Take Down, Double-Edge, Mimic, Double Team, Reflect, Bide, Rest, Substitute, Cut";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Weepinbell(Bellsprout bellsprout)
    : base("Weepinbell", "Grass/Poison", 100, 90, 50, 85, 45, 55, bellsprout.OwnerId ?? "Unknown", 21, "Chlorophyll")
    {
        Id = bellsprout.Id;
        Level = 1;
        Nickname = bellsprout.Nickname;
        Experience = 0;
        HpIV = bellsprout.HpIV;
        AttackIV = bellsprout.AttackIV;
        SpecialAttackIV = bellsprout.SpecialAttackIV;
        DefenseIV = bellsprout.DefenseIV;
        SpecialDefenseIV = bellsprout.SpecialDefenseIV;
        SpeedIV = bellsprout.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;
        SkillPool = "Vine Whip, Sleep Powder, Stun Spore, Acid, Razor Leaf, Growth, Wrap, Poison Powder, Solar Beam, Toxic, Body Slam, Take Down, Double-Edge, Mimic, Double Team, Reflect, Bide, Rest, Substitute, Cut";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }
    
    public Weepinbell(string ownerId)
    : base("Weepinbell", "Grass/Poison", 100, 90, 50, 85, 45, 55, ownerId, 21, "Chlorophyll")
    {
        Nickname = "None";
        SkillPool = "Vine Whip, Sleep Powder, Stun Spore, Acid, Razor Leaf, Growth, Wrap, PoisonPowder, SolarBeam, Toxic, Body Slam, Take Down, Double-Edge, Mimic, Double Team, Reflect, Bide, Rest, Substitute, Cut";

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
        using (var context = new DatabaseContext())
        {
            var item = context.Items.FirstOrDefault(i => i.Name == "Leaf Stone" && i.OwnerId == OwnerId);
            if (item != null)
            {
                context.Items.Remove(item);
            }
            else
            {
                await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} needs a Leaf Stone to evolve!");
                return;
            }

            var victreebel = new Victreebel(this);
            victreebel.MaxHealth = victreebel.HealthOverride;
            victreebel.EvolveLevelUp(Level - 1); // Level up to current level

            foreach (var skill in this.Skills)
            {
                context.Skills.Remove(skill);
            }

            context.PokemonMaster.Remove(this);
            context.PokemonMaster.Add(victreebel);
            foreach (var skill in victreebel.Skills)
            {
                context.Skills.Add(skill);
            }

            // Remove previous and add new Pokemon
            context.SaveChanges();
        }
        await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} has evolved from a Weepinbell to a Victreebel!");
    }

    public override async Task GodEvolve(ClientSession session)
    {
        using (var context = new DatabaseContext())
        {
            var victreebel = new Victreebel(this);
                victreebel.MaxHealth = victreebel.HealthOverride;
            victreebel.EvolveLevelUp(Level-1); // Level up to current level

            foreach (var skill in this.Skills)
            {
                context.Skills.Remove(skill);
            }

            context.PokemonMaster.Remove(this);
            context.PokemonMaster.Add(victreebel);
            foreach (var skill in victreebel.Skills)
            {
                context.Skills.Add(skill);
            }

            // Remove previous and add new Pokemon
            context.SaveChanges();
        }
        await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} has evolved from a Weepinbell to a Victreebel!");
    }

    public override float calculateDamage(float SkillDamage)
    {
        return SkillDamage;
    }
}
