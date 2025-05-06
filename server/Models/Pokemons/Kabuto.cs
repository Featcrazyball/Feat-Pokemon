using Database;
using Server;
namespace PokemonPocket;

public class Kabuto : PokemonMaster
{
    private Kabuto() { } //For EF Core
    public Kabuto(string nickname, string ownerId) 
    : base("Kabuto", "Rock/Water", 30, 80, 90, 55, 45, 55, ownerId, 20, "Battle Armor")
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
                var kabuto = new Kabutops(this);
                kabuto.EvolveLevelUp(Level-1);

                // Remove previous and add new Pokemon
                context.PokemonMaster.Add(kabuto);
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{Nickname} has evolved from a Kabuto to a Kabutops!");
        } else {
            await session.SendMessageAsync($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}