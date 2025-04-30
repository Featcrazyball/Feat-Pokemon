namespace PokemonPocket;

public class Gastly : PokemonMaster
{
    public string? Nickname {get;set;}

    private Gastly() { } //For EF Core
    public Gastly(string nickname, string ownerId) 
    : base("Gastly", "Ghost/Poison", 30, 35, 30, 100, 30, 80, ownerId, 9, "Levitate")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}