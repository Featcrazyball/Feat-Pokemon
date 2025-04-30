namespace PokemonPocket;

public class Haunter : PokemonMaster
{
    public string? Nickname {get;set;}

    private Haunter() { } //For EF Core
    public Haunter(string nickname, string ownerId) 
    : base("Haunter", "Ghost/Poison", 45, 50, 45, 115, 55, 95, ownerId, 25, "Levitate")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}