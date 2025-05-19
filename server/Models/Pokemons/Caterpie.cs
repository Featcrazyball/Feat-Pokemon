using Database;
using Server;
namespace PokemonPocket;

public class Caterpie : PokemonMaster
{
    public override float HealthOverride {get;set;} = 45;
    public override string? Requirements { get; set; } = "Level 7";
    public override string? EvolvesTo {get;set;} = "Metapod";
    private Caterpie() { } //For EF Core
    public Caterpie(string nickname, string ownerId) 
    : base("Caterpie", "Bug", 45, 30, 35, 20, 20, 45, ownerId,  10, "Shield Dust")
    {
        Nickname = nickname;
        SkillPool = "Tackle, String Shot";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }
    
    public Caterpie(string ownerId)
    : base("Caterpie", "Bug", 100, 30, 35, 20, 20, 45, ownerId, 10, "Shield Dust")
    {
        Nickname = "None";
        Experience = 0;
        SkillPool = "Tackle, String Shot";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Caterpie(float HP, string nickname, string ownerId, int exp)
    : base("Caterpie", "Bug", HP, 30, 35, 20, 20, 45, ownerId, 10, "Shield Dust")
    {
        Nickname = nickname;
        Experience = exp;
        SkillPool = "Tackle, String Shot";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills)
            {
                Skills.Add(skill);
            }
            ;
        }
    }

    public override async Task Evolve(ClientSession session)
    {
        if (Level >= 7) {  // Caterpie evolves at level 7
            using (var context = new DatabaseContext())
            {
                var metapod = new Metapod(this);
                metapod.MaxHealth = metapod.HealthOverride;
                metapod.EvolveLevelUp(Level-1);

                foreach (var skill in this.Skills)
                {
                    context.Skills.Remove(skill);
                }

                // Add the evolved Pokemon to the context
                context.PokemonMaster.Remove(this);
                context.PokemonMaster.Add(metapod);
                
                // Add all skills for the evolved Pokemon
                foreach (var skill in metapod.Skills)
                {
                    context.Skills.Add(skill);
                }
                
                // Save all changes in a single transaction
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} has evolved from a Caterpie to a Metapod!");
        } else {
            await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} is not ready to evolve yet.");
        }
    }

    public override async Task GodEvolve(ClientSession session)
    {
        using (var context = new DatabaseContext())
        {
            var metapod = new Metapod(this);
            metapod.MaxHealth = metapod.HealthOverride;
            metapod.EvolveLevelUp(Level-1);

            foreach (var skill in this.Skills)
            {
                context.Skills.Remove(skill);
            }

            // Add the evolved Pokemon to the context
            context.PokemonMaster.Remove(this);
            context.PokemonMaster.Add(metapod);
            
            // Add all skills for the evolved Pokemon
            foreach (var skill in metapod.Skills)
            {
                context.Skills.Add(skill);
            }
            
            // Save all changes in a single transaction
            context.SaveChanges();
        }
        await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} has evolved from a Caterpie to a Metapod!");
    }

    public override float calculateDamage(float SkillDamage)
    {
        return 2 * SkillDamage;
    }
}