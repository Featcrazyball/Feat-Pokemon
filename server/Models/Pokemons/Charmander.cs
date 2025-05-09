using Database;
using Server;
namespace PokemonPocket;
    
public class Charmander : PokemonMaster
{
    public override string? Requirements { get; set; } = "Level 16";
    private Charmander() { } //For EF Core
    public Charmander(string nickname, string ownerId) 
    : base("Charmander", "Fire", 39, 52, 43, 60, 50, 65, ownerId, 10, "Solar Power")
    {
        Nickname = nickname;

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
            foreach (var skill in newSkills) {Skills.Add(skill);};
    }

    public override async Task Evolve(ClientSession session)
    {
        if (Level >= 16) {
            using (var context = new DatabaseContext())
            {
                var charmeleon = new Charmeleon(this);
                charmeleon.EvolveLevelUp(Level-1); 

                // Remove previous and add new Pokemon
                context.PokemonMaster.Add(charmeleon);
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{Nickname} has evolved from a Charmander to a Charmeleon!");
        } else {
            await session.SendMessageAsync($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}