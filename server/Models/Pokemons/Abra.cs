using Database;
using Server;

namespace PokemonPocket;

public class Abra : PokemonMaster
{
    public override string? Requirements { get; set; } = "Level 16";

    private Abra() { } //For EF Core
    public Abra(string nickname, string ownerId) 
    : base("Abra", "Psychic", 25, 20, 15, 105, 55, 90, ownerId, 10, "Synchronize")
    {
        Nickname = nickname;
        SkillPool = "Toxic, Rage, Hyper Beam, SolarBeam, Psychic, Mimic, Double Team, Bide, Swift, Dream Eater, Rest, Psywave, Substitute";

        // Use this to check for null, or else it will throw an error
        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
            foreach (var skill in newSkills) {Skills.Add(skill);};
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }

    public override async Task Evolve(ClientSession session)
    {
        if (Level >= 16) {
            using (var context = new DatabaseContext())
            {
                var kadabra = new Kadabra(this);
                kadabra.EvolveLevelUp(Level-1); // Level up to current level

                // Remove previous and add new Pokemon
                context.PokemonMaster.Add(kadabra);
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{Nickname} has evolved from a Abra to a Kadabra!");
        } else {
            await session.SendMessageAsync($"{Nickname} is not ready to evolve yet.");
        }
    }

}