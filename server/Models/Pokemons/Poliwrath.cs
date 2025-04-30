namespace PokemonPocket;

public class Poliwrath : PokemonMaster
{
    public string? Nickname {get;set;}

    private Poliwrath() { } //For EF Core
    public Poliwrath(string nickname, string ownerId) 
    : base("Poliwrath", "Water", 90, 95, 95, 70, 90, 70, ownerId, 60, "Water Absorb")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}