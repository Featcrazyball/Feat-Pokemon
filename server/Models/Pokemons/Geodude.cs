namespace PokemonPocket;

public class Geodude : PokemonMaster
{
    public string? Nickname {get;set;}

    private Geodude() { } //For EF Core
    public Geodude(string nickname, string ownerId) 
    : base("Geodude", "Rock/Ground", 40, 80, 100, 30, 30, 20, ownerId, 10, "Sturdy")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}