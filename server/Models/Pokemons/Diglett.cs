namespace PokemonPocket;

public class Diglett : PokemonMaster
{
    public string? Nickname {get;set;}

    private Diglett() { } //For EF Core
    public Diglett(string nickname, string ownerId) 
    : base("Diglett", "Ground", 10, 55, 25, 35, 45, 95, ownerId, 10, "Sand Veil")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}