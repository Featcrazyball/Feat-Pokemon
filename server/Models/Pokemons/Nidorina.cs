namespace PokemonPocket;

public class Nidorina : PokemonMaster
{
    public string? Nickname {get;set;}

    private Nidorina() { } //For EF Core
    public Nidorina(string nickname, string ownerId) 
    : base("Nidorina", "Poison", 70, 62, 67, 55, 55, 56, ownerId, 20, "Poison Point")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}