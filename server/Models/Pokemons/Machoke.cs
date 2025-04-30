namespace PokemonPocket;

public class Machoke : PokemonMaster
{
    public string? Nickname {get;set;}

    private Machoke() { } //For EF Core
    public Machoke(string nickname, string ownerId) 
    : base("Machoke", "Fighting", 80, 100, 70, 50, 60, 45, ownerId, 20, "Guts")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}