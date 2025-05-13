using Database;
using Server;
namespace PokemonPocket;

public class Magnemite : PokemonMaster
{
    public override string? Requirements { get; set; } = "Level 30";
    public override string? EvolvesTo {get;set;} = "Magneton";
    private Magnemite() { } //For EF Core
    public Magnemite(string nickname, string ownerId) 
    : base("Magnemite", "Electric/Steel", 25, 35, 70, 95, 55, 45, ownerId, 10, "Magnet Pull")
    {
        Nickname = nickname;
        SkillPool = "Tackle, Sonic Boom, ThunderShock, Supersonic, Thunder Wave, Thunderbolt, Reflect, Toxic, Mimic, Double Team, Bide, Rest, Substitute";

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
                var magnetron = new Magneton(this);
                magnetron.EvolveLevelUp(Level-1); 

                foreach (var skill in this.Skills)
                {
                    context.Skills.Remove(skill);
                }

                context.PokemonMaster.Remove(this);
                context.PokemonMaster.Add(magnetron);
                foreach (var skill in magnetron.Skills)
                {
                    context.Skills.Add(skill);
                }

                // Remove previous and add new Pokemon
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} has evolved from a Magnemite to a Magnetron!");
        } else {
            await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}