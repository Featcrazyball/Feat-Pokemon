namespace PokemonPocket;

public class Nidorino : PokemonMaster
{
    public string? Nickname {get;set;}

    private Nidorino() { } //For EF Core
    public Nidorino(string nickname, string ownerId) 
    : base("Nidorino", "Poison", 61, 72, 57, 55, 55, 65, ownerId, 23, "Poison Point")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}