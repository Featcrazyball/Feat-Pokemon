using Server;
namespace PokemonPocket;

public class Aerodactyl : PokemonMaster
{
    private Aerodactyl() { } //For EF Core
    public Aerodactyl(string nickname, string ownerId) 
    : base("Aerodactyl", "Rock/Flying", 80, 105, 65, 60, 75, 130, ownerId, 20, "Pressure")
    {
        Nickname = nickname;
        SkillPool = "Wing Attack, Supersonic, Bite, Take Down, Agility, Hyper Beam, Toxic, Rage, Earthquake, Mimic, Double-Edge, Skull Bash, Rest, Substitute";

        // Use this to check for null, or else it will throw an error
        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
            foreach (var skill in newSkills) {Skills.Add(skill);};
    }

    public override async Task Evolve(ClientSession session)
    {
        await session.SendMessageAsync($"{Nickname} is already at its final evolution stage.");
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}