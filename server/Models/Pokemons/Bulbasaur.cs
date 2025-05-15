using Database;
using Server;
namespace PokemonPocket;

public class Bulbasaur : PokemonMaster
{
    public override float HealthOverride {get;set;} = 45;
    public override string? Requirements { get; set; } = "Level 16";
    public override string? EvolvesTo {get;set;} = "Ivysaur";

    private Bulbasaur() { } //For EF Core
    public Bulbasaur(string nickname, string ownerId) 
    : base("Bulbasaur", "Grass/Poison", 45, 49, 49, 65, 65, 45, ownerId, 10, "Water Burst")
    {
        Nickname = nickname;
        SkillPool = "Tackle, Growl, Leech Seed, Vine Whip, Poison Powder, Sleep Powder, Razor Leaf, Growth, SolarBeam, Toxic, Body Slam, Take Down, Double-Edge, Rage, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    // Ask Teacher
    public override float calculateDamage(float SkillDamage) {
        return 2*SkillDamage;
    }

    public override async Task Evolve(ClientSession session)
    {
        if (Level >= 16) {  // Bulbasaur evolves at level 16
            using (var context = new DatabaseContext())
            {
                var ivysaur = new Ivysaur(this);
                ivysaur.EvolveLevelUp(Level-1);

                foreach (var skill in this.Skills)
                {
                    context.Skills.Remove(skill);
                }

                // Add the evolved Pokemon to the context
                // Remove the original Pokemon
                context.PokemonMaster.Remove(this);
                context.PokemonMaster.Add(ivysaur);
                
                // Add all skills for the evolved Pokemon
                foreach (var skill in ivysaur.Skills)
                {
                    context.Skills.Add(skill);
                }
                
                
                // Save all changes in a single transaction
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} has evolved from a Bulbasaur to an Ivysaur!");
        } else {
            await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} is not ready to evolve yet.");
        }
    }
}
