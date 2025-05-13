using Database;
using Server;
namespace PokemonPocket;
    
public class Charmander : PokemonMaster
{
    public override string? Requirements { get; set; } = "Level 16";
    public override string? EvolvesTo {get;set;} = "Charmeleon";
    private Charmander() { } //For EF Core
    public Charmander(string nickname, string ownerId) 
    : base("Charmander", "Fire", 39, 52, 43, 60, 50, 65, ownerId, 10, "Solar Power")
    {
        Nickname = nickname;
        SkillPool = "Scratch, Growl, Ember, Leer, Rage, Slash, Flamethrower, Fire Spin, Toxic, Body Slam, Take Down, Double-Edge, Submission, Seismic Toss, Counter, Dragon Rage, Dig, Mimic, Double Team, Reflect, Bide, Fire Blast, Swift, Skull Bash, Rest, Substitute";

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
        if (Level >= 16) {  // Charmander evolves at level 16
            using (var context = new DatabaseContext())
            {
                var charmeleon = new Charmeleon(this);
                charmeleon.EvolveLevelUp(Level-1);

                foreach (var skill in this.Skills)
                {
                    context.Skills.Remove(skill);
                }

                // Add the evolved Pokemon to the context
                context.PokemonMaster.Remove(this);
                context.PokemonMaster.Add(charmeleon);
                
                // Add all skills for the evolved Pokemon
                foreach (var skill in charmeleon.Skills)
                {
                    context.Skills.Add(skill);
                }
                
                // Save all changes in a single transaction
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{Nickname == "None" ? Name : Nickname} has evolved from a Charmander to a Charmeleon!");
        } else {
            await session.SendMessageAsync($"{Nickname == "None" ? Name : Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}