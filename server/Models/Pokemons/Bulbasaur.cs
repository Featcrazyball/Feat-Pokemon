using Database;
using Server;
namespace PokemonPocket;

public class Bulbasaur : PokemonMaster
{
    private Bulbasaur() { } //For EF Core
    public Bulbasaur(string nickname, string ownerId) 
    : base("Bulbasaur", "Grass/Poison", 45, 49, 49, 65, 65, 45, ownerId, 10, "Water Burst")
    {
        Nickname = nickname;

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
            foreach (var skill in newSkills) {Skills.Add(skill);};
    }

    // Ask Teacher
    public override float calculateDamage(float SkillDamage) {
        return 2*SkillDamage;
    }

    public override async Task Evolve(ClientSession session)
    {
        if (Level >= 16) {
            using (var context = new DatabaseContext())
            {
                var ivysaur = new Ivysaur(this);
                ivysaur.EvolveLevelUp(Level-1); 

                // Remove previous and add new Pokemon
                context.PokemonMaster.Add(ivysaur);
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{Nickname} has evolved from a Bulbasaur to a Ivysaur!");
        } else {
            await session.SendMessageAsync($"{Nickname} is not ready to evolve yet.");
        }
    }
}