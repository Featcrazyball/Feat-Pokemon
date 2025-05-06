using Database;
using Server;
namespace PokemonPocket;

public class Magnemite : PokemonMaster
{
    private Magnemite() { } //For EF Core
    public Magnemite(string nickname, string ownerId) 
    : base("Magnemite", "Electric/Steel", 25, 35, 70, 95, 55, 45, ownerId, 10, "Magnet Pull")
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
                var magnetron = new Magnetron(this);
                magnetron.EvolveLevelUp(Level-1); 

                // Remove previous and add new Pokemon
                context.PokemonMaster.Add(magnetron);
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{Nickname} has evolved from a Magnemite to a Magnetron!");
        } else {
            await session.SendMessageAsync($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}