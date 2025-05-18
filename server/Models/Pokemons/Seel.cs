using Database;
using Server;
namespace PokemonPocket;

public class Seel : PokemonMaster
{
    public override float HealthOverride {get;set;} = 65;
    public override string? Requirements { get; set; } = "Level 34";
    public override string? EvolvesTo {get;set;} = "Dewgong";
    private Seel() { } //For EF Core
    public Seel(string nickname, string ownerId) 
    : base("Seel", "Water", 65, 45, 55, 45, 70, 45, ownerId, 15, "Thick Fat")
    {
        Nickname = nickname;
        SkillPool = "Headbutt, Growl, Aurora Beam, Rest, Take Down, Ice Beam, Surf, Body Slam, Toxic, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Seel(float HP, string nickname, string ownerId, int exp)
    : base("Seel", "Water", HP, 45, 55, 45, 70, 45, ownerId, 15, "Thick Fat")
    {
        Nickname = nickname;
        Experience = exp;
        SkillPool = "Headbutt, Growl, Aurora Beam, Rest, Take Down, Ice Beam, Surf, Body Slam, Toxic, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

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
        if (Level >= 34) {
            using (var context = new DatabaseContext())
            {
                var dewgong = new Dewgong(this);
                dewgong.MaxHealth = dewgong.HealthOverride;
                dewgong.EvolveLevelUp(Level-1); 

                foreach (var skill in this.Skills)
                {
                    context.Skills.Remove(skill);
                }

                context.PokemonMaster.Remove(this);
                context.PokemonMaster.Add(dewgong);
                foreach (var skill in dewgong.Skills)
                {
                    context.Skills.Add(skill);
                }

                // Remove previous and add new Pokemon
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} has evolved from a Seel to a Dewgong!");
        } else {
            await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} is not ready to evolve yet.");
        }
    }

    public override async Task GodEvolve(ClientSession session)
    {
        using (var context = new DatabaseContext())
        {
            var dewgong = new Dewgong(this);
            dewgong.MaxHealth = dewgong.HealthOverride;
            dewgong.EvolveLevelUp(Level-1); 

            foreach (var skill in this.Skills)
            {
                context.Skills.Remove(skill);
            }

            context.PokemonMaster.Remove(this);
            context.PokemonMaster.Add(dewgong);
            foreach (var skill in dewgong.Skills)
            {
                context.Skills.Add(skill);
            }

            // Remove previous and add new Pokemon
            context.SaveChanges();
        }
        await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} has evolved from a Seel to a Dewgong!");
    }

    public override float calculateDamage(float SkillDamage)
    {
        return SkillDamage;
    }
}
