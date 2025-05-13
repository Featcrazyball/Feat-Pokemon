using Database;
using Server;
namespace PokemonPocket;

public class Meowth : PokemonMaster
{
    public override string? Requirements { get; set; } = "Level 28";
    private Meowth() { } //For EF Core
    public Meowth(string nickname, string ownerId) 
    : base("Meowth", "Normal", 40, 45, 35, 40, 40, 90, ownerId, 10, "Pickup")
    {
        Nickname = nickname;
        SkillPool = "Scratch, Growl, Bite, Pay Day, Screech, Fury Swipes, Slash, Toxic, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

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
        if (Level >= 28) {
            using (var context = new DatabaseContext())
            {
                var persian = new Persian(this);
                persian.EvolveLevelUp(Level-1);

                foreach (var skill in this.Skills)
                {
                    context.Skills.Remove(skill);
                }

                context.PokemonMaster.Remove(this);
                context.PokemonMaster.Add(persian);
                foreach (var skill in persian.Skills)
                {
                    context.Skills.Add(skill);
                }

                // Remove previous and add new Pokemon
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{Nickname} has evolved from a Meowth to a Persian!");
        } else {
            await session.SendMessageAsync($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}