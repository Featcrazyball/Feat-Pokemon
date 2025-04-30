namespace PokemonPocket;

public class Dodrio : PokemonMaster
{
    public string? Nickname {get;set;}

    private Dodrio() { } //For EF Core
    public Dodrio(string nickname, string ownerId) 
    : base("Dodrio", "Normal/Flying", 60, 110, 70, 60, 60, 110, ownerId, 30, "Early Bird")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}