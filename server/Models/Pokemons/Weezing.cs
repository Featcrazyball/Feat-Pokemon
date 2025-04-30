namespace PokemonPocket;

public class Weezing : PokemonMaster
{
    public string? Nickname {get;set;}

    private Weezing() { } //For EF Core
    public Weezing(string nickname, string ownerId) 
    : base("Weezing", "Poison", 65, 90, 120, 85, 70, 60, ownerId, 35, "Levitate")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}