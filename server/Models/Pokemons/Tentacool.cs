using Database;
using Server;
namespace PokemonPocket;

public class Tentacool : PokemonMaster
{
    public override string? Requirements { get; set; } = "Level 30";
    private Tentacool() { } //For EF Core
    public Tentacool(string nickname, string ownerId) 
    : base("Tentacool", "Water/Poison", 40, 40, 35, 50, 100, 70, ownerId, 10, "Clear Body")
    {
        Nickname = nickname;
        SkillPool = "Acid, Supersonic, Wrap, Poison Sting, Water Gun, Constrict, Barrier, Screech, Toxic, Bubble Beam, Ice Beam, Blizzard, Mimic, Double Team, Reflect, Bide, Rest, Substitute, Surf";

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
                var tentacruel = new Tentacruel(this);
                tentacruel.EvolveLevelUp(Level-1); 

                context.PokemonMaster.Add(tentacruel);
                foreach (var skill in tentacruel.Skills)
                {
                    context.Skills.Add(skill);
                }

                // Remove previous and add new Pokemon
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{Nickname} has evolved from a Tentacool to a Tentacruel!");
        } else {
            await session.SendMessageAsync($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}