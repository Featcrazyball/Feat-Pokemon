using Database;
using Server;
namespace PokemonPocket;

public class Staryu : PokemonMaster
{
    public override string? Requirements { get; set; } = "1 Water Stone";
    public override string? EvolvesTo {get;set;} = "Starmie";
    private Staryu() { } //For EF Core
    public Staryu(string nickname, string ownerId) 
    : base("Staryu", "Water", 30, 45, 55, 70, 55, 85, ownerId, 20, "Illuminate")
    {
        Nickname = nickname;
        SkillPool = "Tackle, Harden, Water Gun, Recover, Swift, Minimize, Light Screen, Hydro Pump, Surf, Thunderbolt, Ice Beam, Blizzard, Psychic, Body Slam, Seismic Toss, Toxic, Mimic, Double Team, Reflect, Bide, Rest, Substitute, Flash";

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
        using (var context = new DatabaseContext())
        {
            var item = context.Items.FirstOrDefault(i => i.Name == "Water Stone" && i.OwnerId == OwnerId);
            if (item != null) {
                context.Items.Remove(item);
            } else {
                await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} needs a Water Stone to evolve!");
                return;
            }

            var starmie = new Starmie(this);
            starmie.EvolveLevelUp(Level-1); // Level up to current level

            foreach (var skill in this.Skills)
            {
                context.Skills.Remove(skill);
            }

            context.PokemonMaster.Remove(this);
            context.PokemonMaster.Add(starmie);
            foreach (var skill in starmie.Skills)
            {
                context.Skills.Add(skill);
            }

            // Remove previous and add new Pokemon
            context.SaveChanges();
        }
        await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} has evolved from a Staryu to a Starmie!");
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}