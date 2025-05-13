using Database;
using Server;
namespace PokemonPocket;

public class Omanyte : PokemonMaster
{
    public override string? Requirements { get; set; } = "Level 40";
    public override string? EvolvesTo {get;set;} = "Omastar";
    private Omanyte() { } //For EF Core
    public Omanyte(string nickname, string ownerId) 
    : base("Omanyte", "Rock/Water", 35, 40, 100, 90, 55, 35, ownerId, 20, "Swift Swim")
    {
        Nickname = nickname;
        SkillPool = "Water Gun, Withdraw, Horn Attack, Leer, Spike Cannon, Hydro Pump, Toxic, Body Slam, Take Down, Double-Edge, Bubble Beam, Ice Beam, Blizzard, Rage, Mimic, Double Team, Reflect, Bide, Rest, Substitute, Surf";

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
                var omastar = new Omastar(this);
                omastar.EvolveLevelUp(Level-1);

                foreach (var skill in this.Skills)
                {
                    context.Skills.Remove(skill);
                }

                context.PokemonMaster.Remove(this);
                context.PokemonMaster.Add(omastar);
                foreach (var skill in omastar.Skills)
                {
                    context.Skills.Add(skill);
                }

                // Remove previous and add new Pokemon
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} has evolved from a Omanyte to a Omastar!");
        } else {
            await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}