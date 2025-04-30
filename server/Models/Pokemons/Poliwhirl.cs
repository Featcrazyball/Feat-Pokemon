namespace PokemonPocket;

public class Poliwhirl : PokemonMaster
{
    public string? Nickname {get;set;}

    private Poliwhirl() { } //For EF Core
    public Poliwhirl(string nickname, string ownerId) 
    : base("Poliwhirl", "Water", 65, 65, 65, 50, 50, 90, ownerId, 25, "Water Absorb")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}