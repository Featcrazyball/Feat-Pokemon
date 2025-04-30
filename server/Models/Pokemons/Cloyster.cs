namespace PokemonPocket;

public class Cloyster : PokemonMaster
{
    public string? Nickname {get;set;}

    private Cloyster() { } //For EF Core
    public Cloyster(string nickname, string ownerId) 
    : base("Cloyster", "Water/Ice", 50, 95, 180, 85, 45, 70, ownerId, 30, "Shell Armor")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}