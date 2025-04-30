namespace PokemonPocket;

public class Omanyte : PokemonMaster
{
    public string? Nickname {get;set;}

    private Omanyte() { } //For EF Core
    public Omanyte(string nickname, string ownerId) 
    : base("Omanyte", "Rock/Water", 35, 40, 100, 90, 55, 35, ownerId, 20, "Swift Swim")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}