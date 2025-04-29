namespace PokemonPocket;

public class Raichu : PokemonMaster
{
    public string? Nickname {get;set;}

    private Raichu() { } //For EF Core
    public Raichu(string nickname, string ownerId) 
    : base("Raichu", "Electric", 60, 90, 55, 90, 80, 110, ownerId, 25, "Thunderbolt")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}