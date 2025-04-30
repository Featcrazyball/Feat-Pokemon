namespace PokemonPocket;

public class Zubat : PokemonMaster
{
    public string? Nickname {get;set;}

    private Zubat() { } //For EF Core
    public Zubat(string nickname, string ownerId) 
    : base("Zubat", "Poison/Flying", 40, 45, 40, 30, 40, 55, ownerId, 10, "Inner Focus")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}