namespace PokemonPocket;

public class Ponyta : PokemonMaster
{
    public string? Nickname {get;set;}

    private Ponyta() { } //For EF Core
    public Ponyta(string nickname, string ownerId) 
    : base("Ponyta", "Fire", 50, 85, 55, 65, 65, 90, ownerId, 20, "Flame Body")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}