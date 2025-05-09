using Database;
using Server;
namespace PokemonPocket;

public class Drowzee : PokemonMaster
{
    public override string? Requirements { get; set; } = "Level 26";
    private Drowzee() { } //For EF Core
    public Drowzee(string nickname, string ownerId) 
    : base("Drowzee", "Psychic", 60, 48, 45, 43, 90, 42, ownerId, 20, "Insomnia")
    {
        Nickname = nickname;

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
            foreach (var skill in newSkills) {Skills.Add(skill);};
    }

    public override async Task Evolve(ClientSession session)
    {
        if (Level >= 26) {
            using (var context = new DatabaseContext())
            {
                var hypno = new Hypno(this);
                hypno.EvolveLevelUp(Level-1); // Level up to current level

                // Remove previous and add new Pokemon
                context.PokemonMaster.Add(hypno);
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{Nickname} has evolved from a Drowzee to a Hypno!");
        } else {
            await session.SendMessageAsync($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}