namespace PokemonPocket;

public class Tentacool : PokemonMaster
{
    public string? Nickname {get;set;}

    private Tentacool() { } //For EF Core
    public Tentacool(string nickname, string ownerId) 
    : base("Tentacool", "Water/Poison", 40, 40, 35, 50, 100, 70, ownerId, 10, "Clear Body")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}