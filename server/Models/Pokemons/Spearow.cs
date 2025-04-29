namespace PokemonPocket;

public class Spearow : PokemonMaster
{
    public string? Nickname {get;set;}

    private Spearow() { } //For EF Core
    public Spearow(string nickname, string ownerId) 
    : base("Spearow", "Normal/Flying", 40, 60, 30, 31, 31, 70, ownerId, 25, "Peck")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return 3*SkillDamage;
    }
}