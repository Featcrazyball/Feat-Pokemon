using Database;
using Server;
namespace PokemonPocket;

public class Horsea : PokemonMaster
{
    public override string? Requirements { get; set; } = "Level 32";
    private Horsea() { } //For EF Core
    public Horsea(string nickname, string ownerId) 
    : base("Horsea", "Water", 30, 40, 70, 70, 25, 60, ownerId, 10, "Swift Swim")
    {
        Nickname = nickname;

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
            foreach (var skill in newSkills) {Skills.Add(skill);};
    }

    public override async Task Evolve(ClientSession session)
    {
        if (Level >= 32) {
            using (var context = new DatabaseContext())
            {
                var seadra = new Seadra(this);
                seadra.EvolveLevelUp(Level-1);

                // Remove previous and add new Pokemon
                context.PokemonMaster.Add(seadra);
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{Nickname} has evolved from a Horsea to a Seadra!");
        } else {
            await session.SendMessageAsync($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}