using Database;
using Server;
namespace PokemonPocket;

public class Slowpoke : PokemonMaster
{
    public override string? Requirements { get; set; } = "Level 37";
    public override string? EvolvesTo {get;set;} = "Slowbro";
    private Slowpoke() { } //For EF Core
    public Slowpoke(string nickname, string ownerId) 
    : base("Slowpoke", "Water/Psychic", 90, 65, 65, 40, 40, 15, ownerId, 20, "Oblivious")
    {
        Nickname = nickname;
        SkillPool = "Confusion, Disable, Headbutt, Growl, Water Gun, Amnesia, Psychic, Surf, Ice Beam, Blizzard, Body Slam, Seismic Toss, Toxic, Mimic, Double Team, Reflect, Bide, Rest, Substitute, Strength";

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
        if (Level >= 37) {
            using (var context = new DatabaseContext())
            {
                var slowbro = new Slowbro(this);
                slowbro.EvolveLevelUp(Level-1); // Level up to current level

                foreach (var skill in this.Skills)
                {
                    context.Skills.Remove(skill);
                }

                context.PokemonMaster.Remove(this);
                context.PokemonMaster.Add(slowbro);
                foreach (var skill in slowbro.Skills)
                {
                    context.Skills.Add(skill);
                }

                // Remove previous and add new Pokemon
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{Nickname == "None" ? Name : Nickname} has evolved from a Slowpoke to a Slowbro!");
        } else {
            await session.SendMessageAsync($"{Nickname == "None" ? Name : Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}