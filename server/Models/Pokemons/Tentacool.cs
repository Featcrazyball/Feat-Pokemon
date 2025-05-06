using Database;
using Server;
namespace PokemonPocket;

public class Tentacool : PokemonMaster
{
    private Tentacool() { } //For EF Core
    public Tentacool(string nickname, string ownerId) 
    : base("Tentacool", "Water/Poison", 40, 40, 35, 50, 100, 70, ownerId, 10, "Clear Body")
    {
        Nickname = nickname;

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
            foreach (var skill in newSkills) {Skills.Add(skill);};
    }

    public override async Task Evolve(ClientSession session)
    {
        if (Level >= 30) {
            using (var context = new DatabaseContext())
            {
                var tentacruel = new Tentacruel(this);
                tentacruel.EvolveLevelUp(Level-1); 

                // Remove previous and add new Pokemon
                context.PokemonMaster.Add(tentacruel);
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{Nickname} has evolved from a Tentacool to a Tentacruel!");
        } else {
            await session.SendMessageAsync($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}