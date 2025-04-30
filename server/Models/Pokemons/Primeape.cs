namespace PokemonPocket;

public class Primeape : PokemonMaster
{
    public string? Nickname {get;set;}

    private Primeape() { } //For EF Core
    public Primeape(string nickname, string ownerId) 
    : base("Primeape", "Fighting", 65, 105, 60, 60, 70, 95, ownerId, 27, "Vital Spirit")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}