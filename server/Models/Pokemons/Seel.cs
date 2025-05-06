using Database;
using Server;
namespace PokemonPocket;

public class Seel : PokemonMaster
{
    private Seel() { } //For EF Core
    public Seel(string nickname, string ownerId) 
    : base("Seel", "Water", 65, 45, 55, 45, 70, 45, ownerId, 15, "Thick Fat")
    {
        Nickname = nickname;

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
            foreach (var skill in newSkills) {Skills.Add(skill);};
    }

    public override async Task Evolve(ClientSession session)
    {
        if (Level >= 34) {
            using (var context = new DatabaseContext())
            {
                var dewgong = new Dewgong(this);
                dewgong.EvolveLevelUp(Level-1); 

                // Remove previous and add new Pokemon
                context.PokemonMaster.Add(dewgong);
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{Nickname} has evolved from a Seel to a Dewgong!");
        } else {
            await session.SendMessageAsync($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}