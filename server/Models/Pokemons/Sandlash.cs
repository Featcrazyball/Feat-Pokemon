namespace PokemonPocket;

public class Sandlash : PokemonMaster
{
    public string? Nickname {get;set;}

    private Sandlash() { } //For EF Core
    public Sandlash(string nickname, string ownerId) 
    : base("Sandlash", "Ground", 75, 100, 110, 45, 55, 65, ownerId, 25, "Sand Attack")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}