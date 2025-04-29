namespace PokemonPocket;

public class Raticate : PokemonMaster
{
    public string? Nickname {get;set;}

    private Raticate() { } //For EF Core
    public Raticate(string nickname, string ownerId) 
    : base("Raticate", "Normal", 55, 81, 60, 50, 70, 97, ownerId, 25, "Run Away")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}