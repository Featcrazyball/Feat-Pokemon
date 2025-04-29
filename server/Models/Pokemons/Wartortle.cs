namespace PokemonPocket;

public class Wartortle : PokemonMaster
{
    public string? Nickname {get;set;}

    private Wartortle() { } //For EF Core
    public Wartortle(string nickname, string ownerId) 
    : base("Wartortle", "Water", 59, 63, 80, 65, 80, 58, ownerId, 25, "Water Gun")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}
