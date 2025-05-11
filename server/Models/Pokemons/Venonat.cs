using Database;
using Server;
namespace PokemonPocket;

public class Venonat : PokemonMaster
{
    public override string? Requirements { get; set; } = "Level 31";
    private Venonat() { } //For EF Core
    public Venonat(string nickname, string ownerId) 
    : base("Venonat", "Bug/Poison", 60, 55, 50, 40, 55, 45, ownerId, 20, "Compound Eyes")
    {
        Nickname = nickname;
        SkillPool = "Tackle, Disable, Supersonic, Confusion, Poison Powder, Leech Life, Stun Spore, Psybeam, Sleep Powder, Toxic, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

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
        if (Level >= 31) {
            using (var context = new DatabaseContext())
            {
                var venomoth = new Venomoth(this);
                venomoth.EvolveLevelUp(Level-1);

                context.PokemonMaster.Add(venomoth);
                foreach (var skill in venomoth.Skills)
                {
                    context.Skills.Add(skill);
                }

                // Remove previous and add new Pokemon
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{Nickname} has evolved from a Venonat to a Venomoth!");
        } else {
            await session.SendMessageAsync($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}