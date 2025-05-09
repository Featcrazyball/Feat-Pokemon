using Database;
using Server;
namespace PokemonPocket;

public class Ponyta : PokemonMaster
{
    public override string? Requirements { get; set; } = "Level 40";
    private Ponyta() { } //For EF Core
    public Ponyta(string nickname, string ownerId) 
    : base("Ponyta", "Fire", 50, 85, 55, 65, 65, 90, ownerId, 20, "Flame Body")
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
                var rapidash = new Rapidash(this);
                rapidash.EvolveLevelUp(Level-1); 

                // Remove previous and add new Pokemon
                context.PokemonMaster.Add(rapidash);
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{Nickname} has evolved from a Ponyta to a Rapidash!");
        } else {
            await session.SendMessageAsync($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}