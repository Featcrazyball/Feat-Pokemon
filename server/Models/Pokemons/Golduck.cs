namespace PokemonPocket;

public class Golduck : PokemonMaster
{
    public string? Nickname {get;set;}

    private Golduck() { } //For EF Core
    public Golduck(string nickname, string ownerId) 
    : base("Golduck", "Water", 80, 82, 78, 95, 80, 85, ownerId, 55, "Damp")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}