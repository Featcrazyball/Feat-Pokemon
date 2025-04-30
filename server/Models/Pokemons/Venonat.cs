namespace PokemonPocket;

public class Venonat : PokemonMaster
{
    public string? Nickname {get;set;}

    private Venonat() { } //For EF Core
    public Venonat(string nickname, string ownerId) 
    : base("Venonat", "Bug/Poison", 60, 55, 50, 40, 55, 45, ownerId, 20, "Compound Eyes")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}