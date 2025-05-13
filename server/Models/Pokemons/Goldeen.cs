using Database;
using Server;
namespace PokemonPocket;

public class Goldeen : PokemonMaster
{
    public override string? Requirements { get; set; } = "Level 33";
    public override string? EvolvesTo {get;set;} = "Seaking";
    private Goldeen() { } //For EF Core
    public Goldeen(string nickname, string ownerId) 
    : base("Goldeen", "Water", 45, 67, 60, 35, 50, 63, ownerId, 20, "Swift Swim")
    {
        Nickname = nickname;
        SkillPool = "Peck, Tail Whip, Supersonic, Horn Attack, Fury Attack, Waterfall, Horn Drill, Agility, Toxic, Body Slam, Take Down, Double-Edge, Rage, Mimic, Double Team, Reflect, Bide, Rest, Substitute, Surf";

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
        if (Level >= 33) {
            using (var context = new DatabaseContext())
            {
                var seaking = new Seaking(this);
                seaking.EvolveLevelUp(Level-1);

                foreach (var skill in this.Skills)
                {
                    context.Skills.Remove(skill);
                }

                context.PokemonMaster.Remove(this);
                context.PokemonMaster.Add(seaking);
                foreach (var skill in seaking.Skills)
                {
                    context.Skills.Add(skill);
                }

                context.SaveChanges();
            }
            await session.SendMessageAsync($"{Nickname == "None" ? Name : Nickname} has evolved from a Goldeen to a Seaking!");
        } else {
            await session.SendMessageAsync($"{Nickname == "None" ? Name : Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}