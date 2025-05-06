using Database;
using Server;
namespace PokemonPocket;

public class Omanyte : PokemonMaster
{
    private Omanyte() { } //For EF Core
    public Omanyte(string nickname, string ownerId) 
    : base("Omanyte", "Rock/Water", 35, 40, 100, 90, 55, 35, ownerId, 20, "Swift Swim")
    {
        Nickname = nickname;

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
            foreach (var skill in newSkills) {Skills.Add(skill);};
    }

    public override async Task Evolve(ClientSession session)
    {
        if (Level >= 40) {
            using (var context = new DatabaseContext())
            {
                var omastar = new Omastar(this);
                omastar.EvolveLevelUp(Level-1);

                // Remove previous and add new Pokemon
                context.PokemonMaster.Add(omastar);
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{Nickname} has evolved from a Omanyte to a Omastar!");
        } else {
            await session.SendMessageAsync($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}