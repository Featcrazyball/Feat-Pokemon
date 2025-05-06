using Database;
using Server;
namespace PokemonPocket;

public class Goldeen : PokemonMaster
{
    private Goldeen() { } //For EF Core
    public Goldeen(string nickname, string ownerId) 
    : base("Goldeen", "Water", 45, 67, 60, 35, 50, 63, ownerId, 20, "Swift Swim")
    {
        Nickname = nickname;

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
            foreach (var skill in newSkills) {Skills.Add(skill);};
    }

    public override async Task Evolve(ClientSession session)
    {
        if (Level >= 33) {
            using (var context = new DatabaseContext())
            {
                var seaking = new Seaking(this);
                seaking.EvolveLevelUp(Level-1);

                // Remove previous and add new Pokemon
                context.PokemonMaster.Add(seaking);
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{Nickname} has evolved from a Goldeen to a Seaking!");
        } else {
            await session.SendMessageAsync($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}