using Database;
using Server;
namespace PokemonPocket;

public class Cubone : PokemonMaster
{
    private Cubone() { } //For EF Core
    public Cubone(string nickname, string ownerId) 
    : base("Cubone", "Ground", 50, 50, 95, 40, 50, 35, ownerId, 20, "Lightning Rod")
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
                var marowak = new Marowak(this);
                marowak.EvolveLevelUp(Level-1);

                // Remove previous and add new Pokemon
                context.PokemonMaster.Add(marowak);
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{Nickname} has evolved from a Cubone to a Marowak!");
        } else {
            await session.SendMessageAsync($"{Nickname} is not ready to evolve yet.");
        }
    }


    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}