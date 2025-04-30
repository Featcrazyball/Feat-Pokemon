namespace PokemonPocket;

public class NidoranF : PokemonMaster
{
    public string? Nickname {get;set;}

    private NidoranF() { } //For EF Core
    public NidoranF(string nickname, string ownerId) 
    : base("NidoranF", "Poison", 55, 47, 52, 40, 40, 41, ownerId, 10, "Poison Point")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}