namespace PokemonPocket;

public class NidoranM : PokemonMaster
{
    public string? Nickname {get;set;}

    private NidoranM() { } //For EF Core
    public NidoranM(string nickname, string ownerId) 
    : base("NidoranM", "Poison", 46, 57, 40, 40, 40, 50, ownerId, 10, "Poison Point")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}