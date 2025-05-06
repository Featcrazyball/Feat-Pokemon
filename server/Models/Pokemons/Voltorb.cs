using Database;
using Server;
namespace PokemonPocket;

public class Voltorb : PokemonMaster
{
    private Voltorb() { } //For EF Core
    public Voltorb(string nickname, string ownerId) 
    : base("Voltorb", "Electric", 40, 30, 50, 55, 55, 100, ownerId, 20, "Static")
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
                var electrode = new Electrode(this);
                electrode.EvolveLevelUp(Level-1); 

                // Remove previous and add new Pokemon
                context.PokemonMaster.Add(electrode);
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{Nickname} has evolved from a Voltorb to an Electrode!");
        } else {
            await session.SendMessageAsync($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}