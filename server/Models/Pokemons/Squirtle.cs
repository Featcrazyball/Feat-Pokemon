using Database;
using Server;
namespace PokemonPocket;

public class Squirtle : PokemonMaster
{
    public override string? Requirements { get; set; } = "Level 16";
    private Squirtle() { } //For EF Core
    public Squirtle(string nickname, string ownerId) 
    : base("Squirtle", "Water", 44, 48, 65, 50, 64, 43, ownerId, 10, "Torrent")
    {
        Nickname = nickname;

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
            foreach (var skill in newSkills) {Skills.Add(skill);};
    }

    public override async Task Evolve(ClientSession session)
    {
        if (Level >= 16) {
            using (var context = new DatabaseContext())
            {
                var wartortle = new Wartortle(this);
                wartortle.EvolveLevelUp(Level-1); // Level up to 16

                // Remove previous and add new Pokemon
                context.PokemonMaster.Add(wartortle);
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{Nickname} has evolved from a Squirtle to a Wartortle!");
        } else {
            await session.SendMessageAsync($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return 2*SkillDamage;
    }
}