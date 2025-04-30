namespace PokemonPocket;

public class Machop : PokemonMaster
{
    public string? Nickname {get;set;}

    private Machop() { } //For EF Core
    public Machop(string nickname, string ownerId) 
    : base("Machop", "Fighting", 70, 80, 50, 35, 35, 35, ownerId, 10, "Guts")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}