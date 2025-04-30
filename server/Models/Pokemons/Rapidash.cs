namespace PokemonPocket;

public class Rapidash : PokemonMaster
{
    public string? Nickname {get;set;}

    private Rapidash() { } //For EF Core
    public Rapidash(string nickname, string ownerId) 
    : base("Rapidash", "Fire", 65, 100, 70, 80, 80, 105, ownerId, 40, "Flame Body")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}