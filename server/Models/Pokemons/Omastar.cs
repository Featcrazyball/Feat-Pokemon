namespace PokemonPocket;

public class Omastar : PokemonMaster
{
    public string? Nickname {get;set;}

    private Omastar() { } //For EF Core
    public Omastar(string nickname, string ownerId) 
    : base("Omastar", "Rock/Water", 70, 60, 125, 115, 70, 55, ownerId, 40, "Swift Swim")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}