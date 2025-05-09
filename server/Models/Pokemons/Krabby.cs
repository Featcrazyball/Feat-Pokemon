using Database;
using Server;
namespace PokemonPocket;

public class Krabby : PokemonMaster
{
    public override string? Requirements { get; set; } = "Level 28";
    private Krabby() { } //For EF Core
    public Krabby(string nickname, string ownerId) 
    : base("Krabby", "Water", 30, 105, 90, 25, 25, 50, ownerId, 10, "Hyper Cutter")
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
                var kingler = new Kingler(this);
                kingler.EvolveLevelUp(Level-1); 

                // Remove previous and add new Pokemon
                context.PokemonMaster.Add(kingler);
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{Nickname} has evolved from a Krabby to a Kingler!");
        } else {
            await session.SendMessageAsync($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}