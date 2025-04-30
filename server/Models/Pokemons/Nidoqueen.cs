namespace PokemonPocket;

public class Nidoqueen : PokemonMaster
{
    public string? Nickname {get;set;}

    private Nidoqueen() { } //For EF Core
    public Nidoqueen(string nickname, string ownerId)
    : base("Nidoqueen", "Poison/Ground", 90, 82, 87, 75, 85, 76, ownerId, 30, "Poison Point")
    {
        Nickname = nickname;
    }
    
    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}