namespace PokemonPocket;

public class Weedle : PokemonMaster
{
    public string? Nickname {get;set;}

    private Weedle() { } //For EF Core
    public Weedle(string nickname, string ownerId) 
    : base("Weedle", "Bug/Poison", 40, 35, 30, 20, 20, 50, ownerId, 10, "Shield Dust")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return 2*SkillDamage;
    }
}