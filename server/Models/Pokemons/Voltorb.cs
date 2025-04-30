namespace PokemonPocket;

public class Voltorb : PokemonMaster
{
    public string? Nickname {get;set;}

    private Voltorb() { } //For EF Core
    public Voltorb(string nickname, string ownerId) 
    : base("Voltorb", "Electric", 40, 30, 50, 55, 55, 100, ownerId, 20, "Static")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}