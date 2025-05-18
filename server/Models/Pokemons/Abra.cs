using Database;
using Server;

namespace PokemonPocket;

public class Abra : PokemonMaster
{
    public override float HealthOverride { get; set; } = 25;
    public override string? Requirements { get; set; } = "Level 16";
    public override string? EvolvesTo { get; set; } = "Kadabra";

    private Abra() { } //For EF Core
    public Abra(string nickname, string ownerId)
    : base("Abra", "Psychic", 25, 20, 15, 105, 55, 90, ownerId, 10, "Synchronize")
    {
        Nickname = nickname;
        SkillPool = "Toxic, Rage, Hyper Beam, SolarBeam, Psychic, Mimic, Double Team, Bide, Swift, Dream Eater, Rest, Psywave, Substitute";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills)
            {
                Skills.Add(skill);
            }
            ;
        }
    }

    public Abra(float HP, string nickname, string ownerId, int exp)
    : base("Abra", "Psychic", HP, 20, 15, 105, 55, 90, ownerId, 10, "Synchronize")
    {
        Nickname = nickname;
        Experience = exp;
        SkillPool = "Toxic, Rage, Hyper Beam, SolarBeam, Psychic, Mimic, Double Team, Bide, Swift, Dream Eater, Rest, Psywave, Substitute";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills)
            {
                Skills.Add(skill);
            }
            ;
        }
    }

    public override float calculateDamage(float SkillDamage)
    {
        return SkillDamage;
    }

    public override async Task Evolve(ClientSession session)
    {
        if (Level >= 16)
        {
            using (var context = new DatabaseContext())
            {
                var kadabra = new Kadabra(this);
                kadabra.MaxHealth = kadabra.HealthOverride;
                kadabra.EvolveLevelUp(Level - 1);

                foreach (var skill in this.Skills)
                {
                    context.Skills.Remove(skill);
                }

                // Add the evolved Pokemon to the context
                context.PokemonMaster.Remove(this);
                context.PokemonMaster.Add(kadabra);

                // Add all skills for the evolved Pokemon
                foreach (var skill in kadabra.Skills)
                {
                    context.Skills.Add(skill);
                }

                // Remove the original Pokemon

                // Save all changes in a single transaction
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} has evolved from Abra to Kadabra!");
        }
        else
        {
            await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} is not ready to evolve yet.");
        }
    }

    public override async Task GodEvolve(ClientSession session)
    {
        using (var context = new DatabaseContext())
        {
            var kadabra = new Kadabra(this);
            kadabra.MaxHealth = kadabra.HealthOverride;
            kadabra.EvolveLevelUp(Level - 1);

            foreach (var skill in this.Skills)
            {
                context.Skills.Remove(skill);
            }

            // Add the evolved Pokemon to the context
            context.PokemonMaster.Remove(this);
            context.PokemonMaster.Add(kadabra);

            // Add all skills for the evolved Pokemon
            foreach (var skill in kadabra.Skills)
            {
                context.Skills.Add(skill);
            }

            // Save all changes in a single transaction
            context.SaveChanges();
        }
        await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} has evolved from Abra to Kadabra!");
    }
}