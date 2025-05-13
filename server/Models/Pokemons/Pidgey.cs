using Database;
using Server;
namespace PokemonPocket;

public class Pidgey : PokemonMaster
{
    public override string? Requirements { get; set; } = "18";
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

    public override async Task Evolve(ClientSession session)
    {
        if (Level >= 18) {
            using (var context = new DatabaseContext())
            {
                var pidgeotto = new Pidgeotto(this);
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
            await session.SendMessageAsync($"{Nickname} has evolved from a Pidgey to a Pidgeotto!");
        } else {
            await session.SendMessageAsync($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return 2*SkillDamage;
    }
}