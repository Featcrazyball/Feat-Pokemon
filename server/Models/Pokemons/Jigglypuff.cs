using Database;
using Server;
namespace PokemonPocket;

public class Jigglypuff : PokemonMaster
{
    public override string? Requirements { get; set; } = "1 Moon Stone";
    public override string? EvolvesTo {get;set;} = "Wigglytuff";
    private Jigglypuff() { } //For EF Core
    public Jigglypuff(string nickname, string ownerId) 
    : base("Jigglypuff", "Normal/Fairy", 115, 45, 20, 45, 25, 25, ownerId, 20, "Cute Charm")
    {
        Nickname = nickname;
        SkillPool = "Sing, Disable, Defense Curl, Double Slap, Rest, Body Slam, Take Down, Double-Edge, Seismic Toss, Counter, Rage, Mimic, Double Team, Reflect, Bide, Fire Blast, Thunderbolt, Thunder, Psychic, Psywave, Substitute";

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
            var item = context.Items.FirstOrDefault(i => i.Name == "Moon Stone" && i.OwnerId == OwnerId);
            if (item != null) {
                context.Items.Remove(item);
            } else {
                await session.SendMessageAsync($"{Nickname == "None" ? Name : Nickname} needs a Moon Stone to evolve!");
                return;
            }

            var wigglytuff = new Wigglytuff(this);
            wigglytuff.EvolveLevelUp(Level-1); // Level up to current level

            foreach (var skill in this.Skills)
            {
                context.Skills.Remove(skill);
            }

            context.PokemonMaster.Remove(this);
            context.PokemonMaster.Add(wigglytuff);
            foreach (var skill in wigglytuff.Skills)
            {
                context.Skills.Add(skill);
            }

            // Remove previous and add new Pokemon
            context.SaveChanges();
        }
        await session.SendMessageAsync($"{Nickname == "None" ? Name : Nickname} has evolved from a Jigglypuff to a Wigglytuff!");
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}