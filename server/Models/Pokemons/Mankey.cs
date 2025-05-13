using Database;
using Server;
namespace PokemonPocket;

public class Mankey : PokemonMaster
{
    public override string? Requirements { get; set; } = "Level 28";
    public override string? EvolvesTo {get;set;} = "Primeape";
    private Mankey() { } //For EF Core
    public Mankey(string nickname, string ownerId) 
    : base("Mankey", "Fighting", 40, 80, 35, 35, 45, 70, ownerId, 14, "Vital Spirit")
    {
        Nickname = nickname;
        SkillPool = "Scratch, Leer, Low Kick, Karate Chop, Fury Swipes, Focus Energy, Seismic Toss, Thrash, Screech, Earthquake, Toxic, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

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
        if (Level >= 28) {
            using (var context = new DatabaseContext())
            {
                var primeape = new Primeape(this);
                primeape.EvolveLevelUp(Level-1); 

                foreach (var skill in this.Skills)
                {
                    context.Skills.Remove(skill);
                }

                context.PokemonMaster.Remove(this);
                context.PokemonMaster.Add(primeape);
                foreach (var skill in primeape.Skills)
                {
                    context.Skills.Add(skill);
                }

                // Remove previous and add new Pokemon
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} has evolved from a Mankey to a Primeape!");
        } else {
            await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}