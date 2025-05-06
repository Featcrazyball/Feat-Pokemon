using Database;
using Server;
namespace PokemonPocket;

public class Venonat : PokemonMaster
{
    private Venonat() { } //For EF Core
    public Venonat(string nickname, string ownerId) 
    : base("Venonat", "Bug/Poison", 60, 55, 50, 40, 55, 45, ownerId, 20, "Compound Eyes")
    {
        Nickname = nickname;

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
            foreach (var skill in newSkills) {Skills.Add(skill);};
    }

    public override async Task Evolve(ClientSession session)
    {
        if (Level >= 31) {
            using (var context = new DatabaseContext())
            {
                var venomoth = new Venomoth(this);
                venomoth.EvolveLevelUp(Level-1);

                // Remove previous and add new Pokemon
                context.PokemonMaster.Add(venomoth);
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{Nickname} has evolved from a Venonat to a Venomoth!");
        } else {
            await session.SendMessageAsync($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}