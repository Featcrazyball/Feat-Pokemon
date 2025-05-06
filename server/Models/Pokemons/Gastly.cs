using Database;
using Server;
namespace PokemonPocket;

public class Gastly : PokemonMaster
{
    private Gastly() { } //For EF Core
    public Gastly(string nickname, string ownerId) 
    : base("Gastly", "Ghost/Poison", 30, 35, 30, 100, 30, 80, ownerId, 9, "Levitate")
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
                var haunter = new Haunter(this);
                haunter.EvolveLevelUp(Level-1); 

                // Remove previous and add new Pokemon
                context.PokemonMaster.Add(haunter);
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{Nickname} has evolved from a Gastly to a Haunter!");
        } else {
            await session.SendMessageAsync($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}