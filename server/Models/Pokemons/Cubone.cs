namespace PokemonPocket;

public class Cubone : PokemonMaster
{
    public string? Nickname {get;set;}

    private Cubone() { } //For EF Core
    public Cubone(string nickname, string ownerId) 
    : base("Cubone", "Ground", 50, 50, 95, 40, 50, 35, ownerId, 20, "Lightning Rod")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}