using Database;
using Server;
namespace PokemonPocket;

public class Oddish : PokemonMaster
{
    public override float HealthOverride {get;set;} = 45;
    public override string? Requirements { get; set; } = "Level 21";
    public override string? EvolvesTo {get;set;} = "Gloom";
    private Oddish() { } //For EF Core
    public Oddish(string nickname, string ownerId) 
    : base("Oddish", "Grass/Poison", 45, 50, 55, 75, 65, 30, ownerId, 10, "Chlorophyll")
    {
        Nickname = nickname;
        SkillPool = "Absorb, PoisonPowder, Stun Spore, Sleep Powder, Acid, SolarBeam, Toxic, Take Down, Double-Edge, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

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
        if (Level >= 21) {
            using (var context = new DatabaseContext())
            {
                var gloom = new Gloom(this);
                gloom.EvolveLevelUp(Level-1);

                foreach (var skill in this.Skills)
                {
                    context.Skills.Remove(skill);
                }

                context.PokemonMaster.Remove(this);
                context.PokemonMaster.Add(gloom);
                foreach (var skill in gloom.Skills)
                {
                    context.Skills.Add(skill);
                }

                // Remove previous and add new Pokemon
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} has evolved from a Oddish to a Gloom!");
        } else {
            await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}
