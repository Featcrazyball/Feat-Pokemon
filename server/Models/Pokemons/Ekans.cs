using Database;
using Server;
namespace PokemonPocket;

public class Ekans : PokemonMaster
{
    private Ekans() { } //For EF Core
    public Ekans(string nickname, string ownerId) 
    : base("Ekans", "Poison", 35, 60, 44, 40, 54, 55, ownerId, 25, "Bite")
    {
        Nickname = nickname;

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
            foreach (var skill in newSkills) {Skills.Add(skill);};
    }

    public override async Task Evolve(ClientSession session)
    {
        if (Level >= 22) {
            using (var context = new DatabaseContext())
            {
                var arbok = new Arbok(this);
                arbok.EvolveLevelUp(Level-1);

                // Remove previous and add new Pokemon
                context.PokemonMaster.Add(arbok);
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{Nickname} has evolved from a Ekans to a Arbok!");
        } else {
            await session.SendMessageAsync($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}