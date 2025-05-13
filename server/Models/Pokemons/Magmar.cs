using Server;
using Database;
namespace PokemonPocket;

public class Magmar : PokemonMaster
{
    public override string? Requirements { get; set; } = "Unevolvable";
    private Magmar() { } //For EF Core
    public Magmar(string nickname, string ownerId) 
    : base("Magmar", "Fire", 65, 95, 57, 100, 85, 93, ownerId, 30, "Flame Body")
    {
        Nickname = nickname;
        SkillPool = "Ember, Leer, Confuse Ray, Fire Punch, Smokescreen, Smog, Flamethrower, Psychic, Fire Blast, Toxic, Mimic, Double Team, Reflect, Bide, Rest, Substitute";
        
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
        await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} is already at its final evolution stage.");
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}