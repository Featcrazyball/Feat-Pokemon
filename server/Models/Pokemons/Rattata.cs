using Database;
using Server;
namespace PokemonPocket;

public class Rattata : PokemonMaster
{
    private Rattata() { } //For EF Core
    public Rattata(string nickname, string ownerId) 
    : base("Rattata", "Normal", 30, 56, 35, 25, 35, 72, ownerId, 25, "Run Away")
    {
        Nickname = nickname;

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
            foreach (var skill in newSkills) {Skills.Add(skill);};
    }

    public override async Task Evolve(ClientSession session)
    {
        if (Level >= 20) {
            using (var context = new DatabaseContext())
            {
                var ratticate = new Raticate(this);
                ratticate.EvolveLevelUp(Level-1); // Level up to 20

                // Remove previous and add new Pokemon
                context.PokemonMaster.Add(ratticate);
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{Nickname} has evolved from a Rattata to a Raticate!");
        } else {
            await session.SendMessageAsync($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}