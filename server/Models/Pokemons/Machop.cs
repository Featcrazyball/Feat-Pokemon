using Database;
using Server;
namespace PokemonPocket;

public class Machop : PokemonMaster
{
    private Machop() { } //For EF Core
    public Machop(string nickname, string ownerId) 
    : base("Machop", "Fighting", 70, 80, 50, 35, 35, 35, ownerId, 10, "Guts")
    {
        Nickname = nickname;

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
            foreach (var skill in newSkills) {Skills.Add(skill);};
    }

    public override async Task Evolve(ClientSession session)
    {
        if (Level >= 28) {
            using (var context = new DatabaseContext())
            {
                var machoke = new Machoke(this);
                machoke.EvolveLevelUp(Level-1); 

                // Remove previous and add new Pokemon
                context.PokemonMaster.Add(machoke);
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{Nickname} has evolved from a Machop to a Machoke!");
        } else {
            await session.SendMessageAsync($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}