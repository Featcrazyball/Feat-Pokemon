using Database;
using Server;
namespace PokemonPocket;

public class NidoranF : PokemonMaster
{
    public override float HealthOverride {get;set;} = 55;
    public override string? Requirements { get; set; } = "Level 16";
    public override string? EvolvesTo {get;set;} = "Nidorina";
    private NidoranF() { } //For EF Core
    public NidoranF(string nickname, string ownerId) 
    : base("NidoranF", "Poison", 55, 47, 52, 40, 40, 41, ownerId, 10, "Poison Point")
    {
        Nickname = nickname;
        SkillPool = "Growl, Tackle, Scratch, Poison Sting, Tail Whip, Bite, Fury Swipes, Double Kick, Toxic, Body Slam, Take Down, Double-Edge, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

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
        if (Level >= 16) {
            using (var context = new DatabaseContext())
            {
                var nidorina = new Nidorina(this);
                nidorina.EvolveLevelUp(Level-1);

                foreach (var skill in this.Skills)
                {
                    context.Skills.Remove(skill);
                }

                context.PokemonMaster.Remove(this);
                context.PokemonMaster.Add(nidorina);
                foreach (var skill in nidorina.Skills)
                {
                    context.Skills.Add(skill);
                }

                // Remove previous and add new Pokemon
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} has evolved from a NidoranF to a Nidorina!");
        } else {
            await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}
