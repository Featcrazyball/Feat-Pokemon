namespace PokemonPocket;

public class Hitmonchan : PokemonMaster
{
    public string? Nickname {get;set;}

    private Hitmonchan() { } //For EF Core
    public Hitmonchan(string nickname, string ownerId) 
    : base("Hitmonchan", "Fighting", 50, 105, 79, 35, 110, 76, ownerId, 20, "Keen Eye")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}