using Database;
using Server;
namespace PokemonPocket;

public class Oddish : PokemonMaster
{
    public override string? Requirements { get; set; } = "Level 21";
    private Oddish() { } //For EF Core
    public Oddish(string nickname, string ownerId) 
    : base("Oddish", "Grass/Poison", 45, 50, 55, 75, 65, 30, ownerId, 10, "Chlorophyll")
    {
        Nickname = nickname;

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
            foreach (var skill in newSkills) {Skills.Add(skill);};
    }

    public override async Task Evolve(ClientSession session)
    {
        if (Level >= 21) {
            using (var context = new DatabaseContext())
            {
                var gloom = new Gloom(this);
                gloom.EvolveLevelUp(Level-1);

                // Remove previous and add new Pokemon
                context.PokemonMaster.Add(gloom);
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{Nickname} has evolved from a Oddish to a Gloom!");
        } else {
            await session.SendMessageAsync($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}