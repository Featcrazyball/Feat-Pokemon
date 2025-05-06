using Database;
using Server;
namespace PokemonPocket;

public class Poliwag : PokemonMaster
{
    private Poliwag() { } //For EF Core
    public Poliwag(string nickname, string ownerId) 
    : base("Poliwag", "Water", 40, 50, 40, 40, 40, 90, ownerId, 16, "Water Absorb")
    {
        Nickname = nickname;

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
            foreach (var skill in newSkills) {Skills.Add(skill);};
    }

    public override async Task Evolve(ClientSession session)
    {
        if (Level >= 25) {
            using (var context = new DatabaseContext())
            {
                var poliwhirl = new Poliwhirl(this);
                poliwhirl.EvolveLevelUp(Level-1); 

                // Remove previous and add new Pokemon
                context.PokemonMaster.Add(poliwhirl);
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{Nickname} has evolved from a Poliwag to a Poliwhirl!");
        } else {
            await session.SendMessageAsync($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}