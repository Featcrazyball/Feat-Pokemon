namespace PokemonPocket;

public class Ekans : PokemonMaster
{
    public string? Nickname {get;set;}

    private Ekans() { } //For EF Core
    public Ekans(string nickname, string ownerId) 
    : base("Ekans", "Poison", 35, 60, 44, 40, 54, 55, ownerId, 25, "Bite")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}