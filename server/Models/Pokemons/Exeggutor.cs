namespace PokemonPocket;

public class Exeggutor : PokemonMaster
{
    public string? Nickname {get;set;}

    private Exeggutor() { } //For EF Core
    public Exeggutor(string nickname, string ownerId) 
    : base("Exeggutor", "Grass/Psychic", 95, 95, 85, 125, 75, 55, ownerId, 30, "Chlorophyll")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}