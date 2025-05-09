using Database;
using Server;
namespace PokemonPocket;

public class Mankey : PokemonMaster
{
    public override string? Requirements { get; set; } = "Level 28";
    private Mankey() { } //For EF Core
    public Mankey(string nickname, string ownerId) 
    : base("Mankey", "Fighting", 40, 80, 35, 35, 45, 70, ownerId, 14, "Vital Spirit")
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
                var primeape = new Primeape(this);
                primeape.EvolveLevelUp(Level-1); 

                // Remove previous and add new Pokemon
                context.PokemonMaster.Add(primeape);
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{Nickname} has evolved from a Mankey to a Primeape!");
        } else {
            await session.SendMessageAsync($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}