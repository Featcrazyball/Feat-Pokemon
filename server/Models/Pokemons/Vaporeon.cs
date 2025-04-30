namespace PokemonPocket;

public class Vaporeon : PokemonMaster
{
    public string? Nickname {get;set;}

    private Vaporeon() { } //For EF Core
    public Vaporeon(string nickname, string ownerId) 
    : base("Vaporeon", "Water", 130, 65, 60, 110, 95, 65, ownerId, 30, "Water Absorb")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}