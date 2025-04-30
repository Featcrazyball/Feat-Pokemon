namespace PokemonPocket;

public class Weepinbell : PokemonMaster
{
    public string? Nickname {get;set;}

    private Weepinbell() { } //For EF Core
    public Weepinbell(string nickname, string ownerId) 
    : base("Weepinbell", "Grass/Poison", 65, 90, 50, 85, 45, 55, ownerId, 21, "Chlorophyll")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}