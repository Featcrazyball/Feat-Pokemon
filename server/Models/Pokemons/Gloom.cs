using Database;
using Server;
namespace PokemonPocket;

public class Gloom : PokemonMaster
{
    public override float HealthOverride {get;set;} = 60;
    public override string? Requirements { get; set; } = "1 Leaf Stone";
    public override string? EvolvesTo {get;set;} = "Vileplume";
    private Gloom() { } //For EF Core
    public Gloom(string nickname, string ownerId) 
    : base("Gloom", "Grass/Poison", 60, 65, 70, 85, 75, 40, ownerId, 20, "Chlorophyll")
    {
        Nickname = nickname;
        SkillPool = "Absorb, Poison Powder, Stun Spore, Sleep Powder, Acid, Petal Dance, Solar Beam, Toxic, Body Slam, Take Down, Double-Edge, Rage, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Gloom(float HP, string nickname, string ownerId, int exp)
    : base("Gloom", "Grass/Poison", HP, 65, 70, 85, 75, 40, ownerId, 20, "Chlorophyll")
    {
        Nickname = nickname;
        Experience = exp;
        SkillPool = "Absorb, Poison Powder, Stun Spore, Sleep Powder, Acid, Petal Dance, Solar Beam, Toxic, Body Slam, Take Down, Double-Edge, Rage, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Gloom(Oddish oddish)
    : base("Gloom", "Grass/Poison", 100, 65, 70, 85, 75, 40, oddish.OwnerId ?? "Unknown", 20, "Chlorophyll")
    {
        Id = oddish.Id;
        Level = 1;
        Nickname = oddish.Nickname;
        Experience = 0;
        HpIV = oddish.HpIV;
        AttackIV = oddish.AttackIV;
        SpecialAttackIV = oddish.SpecialAttackIV;
        DefenseIV = oddish.DefenseIV;
        SpecialDefenseIV = oddish.SpecialDefenseIV;
        SpeedIV = oddish.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;
        SkillPool = "Absorb, Poison Powder, Stun Spore, Sleep Powder, Acid, Petal Dance, Solar Beam, Toxic, Body Slam, Take Down, Double-Edge, Rage, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }
    
    public Gloom(string ownerId) 
    : base("Gloom", "Grass/Poison", 100, 65, 70, 85, 75, 40, ownerId, 20, "Chlorophyll")
    {
        Nickname = "None";
        SkillPool = "Absorb, Poison Powder, Stun Spore, Sleep Powder, Acid, Petal Dance, Solar Beam, Toxic, Body Slam, Take Down, Double-Edge, Rage, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

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

            var Vileplume = new Vileplume(this);
            Vileplume.MaxHealth = Vileplume.HealthOverride;
            Vileplume.EvolveLevelUp(Level - 1); // Level up to current level

            foreach (var skill in this.Skills)
            {
                context.Skills.Remove(skill);
            }

            context.PokemonMaster.Remove(this);
            context.PokemonMaster.Add(Vileplume);
            foreach (var skill in Vileplume.Skills)
            {
                context.Skills.Add(skill);
            }

            context.SaveChanges();
        }
        await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} has evolved from a Gloom to a Vileplume!");
    }

    public override async Task GodEvolve(ClientSession session)
    {
        using (var context = new DatabaseContext())
        {
            var Vileplume = new Vileplume(this);
                Vileplume.MaxHealth = Vileplume.HealthOverride;
            Vileplume.EvolveLevelUp(Level-1); // Level up to current level

            foreach (var skill in this.Skills)
            {
                context.Skills.Remove(skill);
            }

            context.PokemonMaster.Remove(this);
            context.PokemonMaster.Add(Vileplume);
            foreach (var skill in Vileplume.Skills)
            {
                context.Skills.Add(skill);
            }

            context.SaveChanges();
        }
        await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} has evolved from a Gloom to a Vileplume!");
    }

    public override float calculateDamage(float SkillDamage)
    {
        return SkillDamage;
    }
}
