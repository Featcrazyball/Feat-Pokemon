namespace PokemonPocket;

public class Poliwag : PokemonMaster
{
    public string? Nickname {get;set;}

    private Poliwag() { } //For EF Core
    public Poliwag(string nickname, string ownerId) 
    : base("Poliwag", "Water", 40, 50, 40, 40, 40, 90, ownerId, 16, "Water Absorb")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}