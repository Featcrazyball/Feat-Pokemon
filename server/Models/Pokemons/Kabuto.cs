using Database;
using Server;
namespace PokemonPocket;

public class Kabuto : PokemonMaster
{
    public override string? Requirements { get; set; } = "Level 40";
    private Kabuto() { } //For EF Core
    public Kabuto(string nickname, string ownerId) 
    : base("Kabuto", "Rock/Water", 30, 80, 90, 55, 45, 55, ownerId, 20, "Battle Armor")
    {
        Nickname = nickname;
        SkillPool = "Scratch, Harden, Absorb, Slash, Leer, Hydro Pump, Toxic, Body Slam, Take Down, Double-Edge, BubbleBeam, Ice Beam, Blizzard, Rage, Mimic, Double Team, Reflect, Bide, Rest, Substitute, Surf";

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
        if (Level >= 40) {
            using (var context = new DatabaseContext())
            {
                var kabuto = new Kabutops(this);
                kabuto.EvolveLevelUp(Level-1);

                foreach (var skill in this.Skills)
                {
                    context.Skills.Remove(skill);
                }

                context.PokemonMaster.Remove(this);
                context.PokemonMaster.Add(kabuto);
                foreach (var skill in kabuto.Skills)
                {
                    context.Skills.Add(skill);
                }

                // Remove previous and add new Pokemon
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{Nickname} has evolved from a Kabuto to a Kabutops!");
        } else {
            await session.SendMessageAsync($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}