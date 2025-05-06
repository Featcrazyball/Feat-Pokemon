using Database;
using Server;
namespace PokemonPocket;

public class Zubat : PokemonMaster
{
    private Zubat() { } //For EF Core
    public Zubat(string nickname, string ownerId) 
    : base("Zubat", "Poison/Flying", 40, 45, 40, 30, 40, 55, ownerId, 10, "Inner Focus")
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
                var golbat = new Golbat(this);
                golbat.EvolveLevelUp(Level-1);

                // Remove previous and add new Pokemon
                context.PokemonMaster.Add(golbat);
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{Nickname} has evolved from a Zubat to a Golbat!");
        } else {
            await session.SendMessageAsync($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}