namespace PokemonPocket;

public class Charizard : PokemonMaster
{
    public string? Nickname {get;set;}

    private Charizard() { } //For EF Core
    public Charizard(string nickname, string ownerId) 
    : base("Charizard", "Fire/Flying", 78, 84, 78, 109, 85, 100, ownerId, 40, "Fire Burst")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}
