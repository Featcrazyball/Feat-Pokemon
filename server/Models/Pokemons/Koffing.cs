namespace PokemonPocket;

public class Koffing : PokemonMaster
{
    public string? Nickname {get;set;}

    private Koffing() { } //For EF Core
    public Koffing(string nickname, string ownerId) 
    : base("Koffing", "Poison", 40, 65, 95, 60, 45, 35, ownerId, 35, "Levitate")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}