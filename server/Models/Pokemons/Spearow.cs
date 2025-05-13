using Database;
using Server;
namespace PokemonPocket;

public class Spearow : PokemonMaster
{
    public override string? Requirements { get; set; } = "Level 20";
    private Spearow() { } //For EF Core
    public Spearow(string nickname, string ownerId) 
    : base("Spearow", "Normal/Flying", 40, 60, 30, 31, 31, 70, ownerId, 25, "Peck")
    {
        Nickname = nickname;
        SkillPool = "Peck, Growl, Leer, Fury Attack, Agility, Drill Peck, Toxic, Mimic, Double Team, Reflect, Bide, Rest, Substitute, Fly";

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
        if (Level >= 20) {
            using (var context = new DatabaseContext())
            {
                var fearow = new Fearow(this);
                fearow.EvolveLevelUp(Level-1); // Level up to 20

                foreach (var skill in this.Skills)
                {
                    context.Skills.Remove(skill);
                }

                context.PokemonMaster.Remove(this);
                context.PokemonMaster.Add(fearow);
                foreach (var skill in fearow.Skills)
                {
                    context.Skills.Add(skill);
                }

                // Remove previous and add new Pokemon
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{Nickname} has evolved from a Spearow to a Fearow!");
        } else {
            await session.SendMessageAsync($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return 3*SkillDamage;
    }
}