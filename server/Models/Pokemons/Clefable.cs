namespace PokemonPocket;

public class Clefable : PokemonMaster
{
    public string? Nickname {get;set;}

    private Clefable() { } //For EF Core
    public Clefable(string nickname, string ownerId) 
    : base("Clefable", "Fairy", 95, 70, 73, 95, 90, 60, ownerId, 35, "Cute Charm")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}