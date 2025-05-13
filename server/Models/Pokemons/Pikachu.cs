using Database;
using Server;
namespace PokemonPocket;

public class Pikachu : PokemonMaster
{
    public override string? Requirements { get; set; } = "1 Thunder Stone";
    public override string? EvolvesTo {get;set;} = "Raichu";
    private Pikachu() { } //For EF Core
    public Pikachu(string nickname, string ownerId) 
    : base("Pikachu", "Electric", 35, 55, 40, 50, 50, 90, ownerId, 30, "Lightning Bolt")
    {
        Nickname = nickname;
        SkillPool = "Thunder Shock, Growl, Thunder Wave, Quick Attack, Swift, Agility, Thunder, Body Slam, Take Down, Double-Edge, Seismic Toss, Thunderbolt, Mimic, Double Team, Reflect, Bide, Rest, Substitute, Surf";

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
            var item = context.Items.FirstOrDefault(i => i.Name == "Thunder Stone" && i.OwnerId == OwnerId);
            if (item != null) {
                context.Items.Remove(item);
            } else {
                await session.SendMessageAsync($"{Nickname == "None" ? Name : Nickname} needs a Thunderstone to evolve!");
                return;
            }

            var raichu = new Raichu(this);
            raichu.EvolveLevelUp(Level-1); // Level up to current level

            foreach (var skill in this.Skills)
            {
                context.Skills.Remove(skill);
            }

            context.PokemonMaster.Remove(this);
            context.PokemonMaster.Add(raichu);
            foreach (var skill in raichu.Skills)
            {
                context.Skills.Add(skill);
            }

            // Remove previous and add new Pokemon
            context.SaveChanges();
        }
        await session.SendMessageAsync($"{Nickname == "None" ? Name : Nickname} has evolved from a Pikachu to a Raichu!");
    }

    public override float calculateDamage(float SkillDamage) {
        return 3*SkillDamage;
    }
}