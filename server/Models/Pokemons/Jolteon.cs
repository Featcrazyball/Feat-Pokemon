namespace PokemonPocket;

public class Jolteon : PokemonMaster
{
    public string? Nickname {get;set;}

    private Jolteon() { } //For EF Core
    public Jolteon(string nickname, string ownerId) 
    : base("Jolteon", "Electric", 65, 65, 60, 110, 95, 130, ownerId, 29, "Volt Absorb")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}