namespace PokemonPocket;

public class Muk : PokemonMaster
{
    public string? Nickname {get;set;}

    private Muk() { } //For EF Core
    public Muk(string nickname, string ownerId) 
    : base("Muk", "Poison", 105, 105, 75, 65, 100, 50, ownerId, 35, "Poison Touch")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}