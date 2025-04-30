namespace PokemonPocket;

public class Dragonair : PokemonMaster
{
    public string? Nickname {get;set;}

    private Dragonair() { } //For EF Core
    public Dragonair(string nickname, string ownerId) 
    : base("Dragonair", "Dragon", 61, 84, 65, 70, 70, 70, ownerId, 30, "Shed Skin")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}