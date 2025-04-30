namespace PokemonPocket;

public class Golem : PokemonMaster
{
    public string? Nickname {get;set;}

    private Golem() { } //For EF Core
    public Golem(string nickname, string ownerId) 
    : base("Golem", "Rock/Ground", 80, 120, 130, 55, 65, 45, ownerId, 43, "Sturdy")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}