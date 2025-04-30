namespace PokemonPocket;

public class Jigglypuff : PokemonMaster
{
    public string? Nickname {get;set;}

    private Jigglypuff() { } //For EF Core
    public Jigglypuff(string nickname, string ownerId) 
    : base("Jigglypuff", "Normal/Fairy", 115, 45, 20, 45, 25, 25, ownerId, 20, "Cute Charm")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}