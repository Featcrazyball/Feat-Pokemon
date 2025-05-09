using Database;
using Server;
namespace PokemonPocket;

public class Psyduck : PokemonMaster
{
    public override string? Requirements { get; set; } = "Level 33";
    private Psyduck() { } //For EF Core
    public Psyduck(string nickname, string ownerId) 
    : base("Psyduck", "Water", 50, 52, 48, 65, 50, 55, ownerId, 33, "Damp")
    {
        Nickname = nickname;

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
            foreach (var skill in newSkills) {Skills.Add(skill);};
    }

    public override async Task Evolve(ClientSession session)
    {
        if (Level >= 33) {
            using (var context = new DatabaseContext())
            {
                var golduck = new Golduck(this);
                golduck.EvolveLevelUp(Level-1);

                // Remove previous and add new Pokemon
                context.PokemonMaster.Add(golduck);
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{Nickname} has evolved from a Psyduck to a Golduck!");
        } else {
            await session.SendMessageAsync($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}