namespace PokemonPocket;

public class Fearow : PokemonMaster
{
    public string? Nickname {get;set;}

    private Fearow() { } //For EF Core
    public Fearow(string nickname, string ownerId) 
    : base("Fearow", "Normal/Flying", 65, 90, 65, 61, 61, 100, ownerId, 25, "Peck")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}