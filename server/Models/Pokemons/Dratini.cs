namespace PokemonPocket;

public class Dratini : PokemonMaster
{
    public string? Nickname {get;set;}

    private Dratini() { } //For EF Core
    public Dratini(string nickname, string ownerId) 
    : base("Dratini", "Dragon", 41, 64, 45, 50, 50, 50, ownerId, 15, "Shed Skin")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}