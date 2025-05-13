using Database;
using Server;
namespace PokemonPocket;

public class Dratini : PokemonMaster
{
    public override string? Requirements { get; set; } = "Level 30";
    public override string? EvolvesTo {get;set;} = "Dragonair";
    private Dratini() { } //For EF Core
    public Dratini(string nickname, string ownerId) 
    : base("Dratini", "Dragon", 41, 64, 45, 50, 50, 50, ownerId, 15, "Shed Skin")
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

    public override async Task Evolve(ClientSession session)
    {
        if (Level >= 30) {
            using (var context = new DatabaseContext())
            {
                var dragonair = new Dragonair(this);
                dragonair.EvolveLevelUp(Level-1);

                foreach (var skill in this.Skills)
                {
                    context.Skills.Remove(skill);
                }

                // Add skills for the evolved Pokemon
                context.PokemonMaster.Remove(this);
                context.PokemonMaster.Add(dragonair);
                foreach (var skill in dragonair.Skills)
                {
                    context.Skills.Add(skill);
                }
                
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} has evolved from a Dratini to a Dragonair!");
        } else {
            await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}