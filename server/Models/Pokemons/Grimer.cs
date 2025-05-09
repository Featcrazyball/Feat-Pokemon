using Database;
using Server;
namespace PokemonPocket;

public class Grimer : PokemonMaster
{
    public override string? Requirements { get; set; } = "Level 38";
    private Grimer() { } //For EF Core
    public Grimer(string nickname, string ownerId) 
    : base("Grimer", "Poison", 80, 80, 50, 40, 50, 25, ownerId, 15, "Poison Touch")
    {
        Nickname = nickname;

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
            foreach (var skill in newSkills) {Skills.Add(skill);};
    }

    public override async Task Evolve(ClientSession session)
    {
        if (Level >= 38) {
            using (var context = new DatabaseContext())
            {
                var muk = new Muk(this);
                muk.EvolveLevelUp(Level-1); 

                // Remove previous and add new Pokemon
                context.PokemonMaster.Add(muk);
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{Nickname} has evolved from a Grimer to a Muk!");
        } else {
            await session.SendMessageAsync($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}