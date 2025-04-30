namespace PokemonPocket;

public class Machamp : PokemonMaster
{
    public string? Nickname {get;set;}

    private Machamp() { } //For EF Core
    public Machamp(string nickname, string ownerId) 
    : base("Machamp", "Fighting", 90, 130, 80, 65, 85, 55, ownerId, 20, "No Guard")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}