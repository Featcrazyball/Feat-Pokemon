namespace PokemonPocket;

public class Eevee : PokemonMaster
{
    public string? Nickname {get;set;}

    private Eevee() { } //For EF Core
    public Eevee(string nickname, string ownerId) 
    : base("Eevee", "Normal", 55, 55, 50, 45, 65, 55, ownerId, 25, "Run Away")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return 2*SkillDamage;
    }
}