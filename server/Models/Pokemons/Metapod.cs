namespace PokemonPocket;

public class Metapod : PokemonMaster
{
    public string? Nickname {get;set;}

    private Metapod() { } //For EF Core
    public Metapod(string nickname, string ownerId) 
    : base("Metapod", "Bug", 50, 20, 55, 25, 25, 30, ownerId, 25, "Harden")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}