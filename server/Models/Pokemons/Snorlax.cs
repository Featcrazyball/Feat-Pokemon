namespace PokemonPocket;

public class Snorlax : PokemonMaster
{
    public string? Nickname {get;set;}

    private Snorlax() { } //For EF Core
    public Snorlax(string nickname, string ownerId) 
    : base("Snorlax", "Normal", 160, 110, 65, 65, 110, 30, ownerId, 30, "Immunity")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}