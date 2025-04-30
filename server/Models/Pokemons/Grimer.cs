namespace PokemonPocket;

public class Grimer : PokemonMaster
{
    public string? Nickname {get;set;}

    private Grimer() { } //For EF Core
    public Grimer(string nickname, string ownerId) 
    : base("Grimer", "Poison", 80, 80, 50, 40, 50, 25, ownerId, 15, "Poison Touch")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}