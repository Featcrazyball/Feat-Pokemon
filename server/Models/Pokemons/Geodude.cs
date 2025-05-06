using Database;
using Server;
namespace PokemonPocket;

public class Geodude : PokemonMaster
{
    private Geodude() { } //For EF Core
    public Geodude(string nickname, string ownerId) 
    : base("Geodude", "Rock/Ground", 40, 80, 100, 30, 30, 20, ownerId, 10, "Sturdy")
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
                var graveler = new Graveler(this);
                graveler.EvolveLevelUp(Level-1); 

                // Remove previous and add new Pokemon
                context.PokemonMaster.Add(graveler);
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{Nickname} has evolved from a Geodude to a Graveler!");
        } else {
            await session.SendMessageAsync($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}