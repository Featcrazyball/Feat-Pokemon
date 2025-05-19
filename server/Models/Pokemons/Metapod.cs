using Database;
using Server;
namespace PokemonPocket;

public class Metapod : PokemonMaster
{
    public override float HealthOverride {get;set;} = 50;
    public override string? Requirements { get; set; } = "Level 10";
    public override string? EvolvesTo {get;set;} = "Butterfree";
    private Metapod() { } //For EF Core
    public Metapod(string nickname, string ownerId) 
    : base("Metapod", "Bug", 50, 20, 55, 25, 25, 30, ownerId, 25, "Harden")
    {
        Nickname = nickname;
        SkillPool = "Harden";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Metapod(float HP, string nickname, string ownerId, int exp)
    : base("Metapod", "Bug", HP, 20, 55, 25, 25, 30, ownerId, 25, "Harden")
    {
        Nickname = nickname;
        Experience = exp;
        SkillPool = "Harden";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Metapod(Caterpie caterpie)
    : base("Metapod", "Bug", 100, 20, 55, 25, 25, 30, caterpie.OwnerId ?? "Unknown", 25, "Harden")
    {
        Id = caterpie.Id;
        Level = 1;
        Nickname = caterpie.Nickname;
        Experience = 0;
        HpIV = caterpie.HpIV;
        AttackIV = caterpie.AttackIV;
        SpecialAttackIV = caterpie.SpecialAttackIV;
        DefenseIV = caterpie.DefenseIV;
        SpecialDefenseIV = caterpie.SpecialDefenseIV;
        SpeedIV = caterpie.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;
        SkillPool = "Harden";

        using (var context = new DatabaseContext())
        {
            var newSkills = LearnSkillFromSkillPool();
            if (newSkills != null)
            {
                foreach (var skill in newSkills) 
                {
                    Skills.Add(skill);
                    context.Skills.Add(skill);
                };
                context.SaveChanges();
            }
        }
    }
    
    public Metapod(string ownerId)
    : base("Metapod", "Bug", 100, 20, 55, 25, 25, 30, ownerId, 25, "Harden")
    {
        Nickname = "None";
        Experience = 0;
        SkillPool = "Harden";

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
        if (Level >= 10)
        {
            using (var context = new DatabaseContext())
            {
                var butterfree = new Butterfree(this);
                butterfree.MaxHealth = butterfree.HealthOverride;
                butterfree.EvolveLevelUp(Level - 1); // Level up to 10

                foreach (var skill in this.Skills)
                {
                    context.Skills.Remove(skill);
                }

                context.PokemonMaster.Remove(this);
                context.PokemonMaster.Add(butterfree);
                foreach (var skill in butterfree.Skills)
                {
                    context.Skills.Add(skill);
                }

                // Remove previous and add new Pokemon
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} has evolved from a Metapod to a Butterfree!");
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
            var butterfree = new Butterfree(this);
            butterfree.MaxHealth = butterfree.HealthOverride;
            butterfree.EvolveLevelUp(Level-1); // Level up to 10

            foreach (var skill in this.Skills)
            {
                context.Skills.Remove(skill);
            }

            context.PokemonMaster.Remove(this);
            context.PokemonMaster.Add(butterfree);
            foreach (var skill in butterfree.Skills)
            {
                context.Skills.Add(skill);
            }

            // Remove previous and add new Pokemon
            context.SaveChanges();
        }
        await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} has evolved from a Metapod to a Butterfree!");
    }

    public override float calculateDamage(float SkillDamage)
    {
        return SkillDamage;
    }
}
