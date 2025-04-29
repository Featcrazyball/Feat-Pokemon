namespace PokemonPocket;

public class Rattata : PokemonMaster
{
    public string? Nickname {get;set;}

    private Rattata() { } //For EF Core
    public Rattata(string nickname, string ownerId) 
    : base("Rattata", "Normal", 30, 56, 35, 25, 35, 72, ownerId, 25, "Run Away")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}