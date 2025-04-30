namespace PokemonPocket;

public class Nidoking : PokemonMaster
{
    public string? Nickname {get;set;}

    private Nidoking() { } //For EF Core
    public Nidoking(string nickname, string ownerId)
    : base("Nidoking", "Poison/Ground", 81, 102, 77, 85, 75, 85, ownerId, 30, "Poison Point")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}