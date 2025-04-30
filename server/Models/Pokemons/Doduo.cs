namespace PokemonPocket;

public class Doduo : PokemonMaster
{
    public string? Nickname {get;set;}

    private Doduo() { } //For EF Core
    public Doduo(string nickname, string ownerId) 
    : base("Doduo", "Normal/Flying", 35, 85, 45, 35, 35, 75, ownerId, 20, "Run Away")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}