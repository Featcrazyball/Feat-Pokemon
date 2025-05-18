using Database;
using Server;
namespace PokemonPocket;

public class Pidgey : PokemonMaster
{
    public override float HealthOverride {get;set;} = 40;
    public override string? Requirements { get; set; } = "Level 18";
    public override string? EvolvesTo {get;set;} = "Pidgeotto";
    private Pidgey() { } //For EF Core
    public Pidgey(string nickname, string ownerId) 
    : base("Pidgey", "Normal/Flying", 40, 45, 40, 35, 35, 56, ownerId, 10, "Keen Eye")
    {
        Nickname = nickname;
        SkillPool = "Gust, Sand Attack, Quick Attack, Whirlwind, Wing Attack, Toxic, Mimic, Double Team, Reflect, Bide, Rest, Substitute, Fly";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Pidgey(float HP, string nickname, string ownerId, int exp)
    : base("Pidgey", "Normal/Flying", HP, 45, 40, 35, 35, 56, ownerId, 10, "Keen Eye")
    {
        Nickname = nickname;
        Experience = exp;
        SkillPool = "Gust, Sand Attack, Quick Attack, Whirlwind, Wing Attack, Toxic, Mimic, Double Team, Reflect, Bide, Rest, Substitute, Fly";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public override async Task Evolve(ClientSession session)
    {
        if (Level >= 18) {
            using (var context = new DatabaseContext())
            {
                var pidgeotto = new Pidgeotto(this);
                pidgeotto.MaxHealth = pidgeotto.HealthOverride;
                pidgeotto.EvolveLevelUp(Level-1); // Level up to 18

                foreach (var skill in this.Skills)
                {
                    context.Skills.Remove(skill);
                }

                context.PokemonMaster.Remove(this);
                context.PokemonMaster.Add(pidgeotto);
                foreach (var skill in pidgeotto.Skills)
                {
                    context.Skills.Add(skill);
                }

                // Remove previous and add new Pokemon
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} has evolved from a Pidgey to a Pidgeotto!");
        } else {
            await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} is not ready to evolve yet.");
        }
    }

    public override async Task GodEvolve(ClientSession session)
    {
        using (var context = new DatabaseContext())
        {
            var pidgeotto = new Pidgeotto(this);
            pidgeotto.MaxHealth = pidgeotto.HealthOverride;
            pidgeotto.EvolveLevelUp(Level-1); // Level up to 18

            foreach (var skill in this.Skills)
            {
                context.Skills.Remove(skill);
            }

            context.PokemonMaster.Remove(this);
            context.PokemonMaster.Add(pidgeotto);
            foreach (var skill in pidgeotto.Skills)
            {
                context.Skills.Add(skill);
            }

            // Remove previous and add new Pokemon
            context.SaveChanges();
        }
        await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} has evolved from a Pidgey to a Pidgeotto!");
    }

    public override float calculateDamage(float SkillDamage)
    {
        return 2 * SkillDamage;
    }
}
