namespace PokemonPocket;

public class Pikachu : PokemonMaster
{
    public string? Nickname {get;set;}

    private Pikachu() { } //For EF Core
    public Pikachu(string nickname, string ownerId) 
    : base("Pikachu", "Electric", 35, 55, 40, 50, 50, 90, ownerId, 30, "Lightning Bolt")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return 3*SkillDamage;
    }
}