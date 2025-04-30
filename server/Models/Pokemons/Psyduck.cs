namespace PokemonPocket;

public class Psyduck : PokemonMaster
{
    public string? Nickname {get;set;}

    private Psyduck() { } //For EF Core
    public Psyduck(string nickname, string ownerId) 
    : base("Psyduck", "Water", 50, 52, 48, 65, 50, 55, ownerId, 33, "Damp")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}