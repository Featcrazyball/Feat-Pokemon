using Database;
using Server;
namespace PokemonPocket;

public class Horsea : PokemonMaster
{
    public override string? Requirements { get; set; } = "Level 32";
    private Horsea() { } //For EF Core
    public Horsea(string nickname, string ownerId) 
    : base("Horsea", "Water", 30, 40, 70, 70, 25, 60, ownerId, 10, "Swift Swim")
    {
        Nickname = nickname;
        SkillPool = "Bubble, Smokescreen, Leer, Water Gun, Agility, Hydro Pump, Toxic, Blizzard, Rage, Mimic, Double Team, Reflect, Bide, Rest, Substitute, Surf";

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
        if (Level >= 32) {
            using (var context = new DatabaseContext())
            {
                var seadra = new Seadra(this);
                seadra.EvolveLevelUp(Level-1);

                foreach (var skill in this.Skills)
                {
                    context.Skills.Remove(skill);
                }

                context.PokemonMaster.Remove(this);
                context.PokemonMaster.Add(seadra);
                foreach (var skill in seadra.Skills)
                {
                    context.Skills.Add(skill);
                }

                // Remove previous and add new Pokemon
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{Nickname} has evolved from a Horsea to a Seadra!");
        } else {
            await session.SendMessageAsync($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}