namespace PokemonPocket;

public class Chansey : PokemonMaster
{
    public string? Nickname {get;set;}

    private Chansey() { } //For EF Core
    public Chansey(string nickname, string ownerId) 
    : base("Chansey", "Normal", 250, 5, 5, 35, 105, 50, ownerId, 30, "Natural Cure")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}