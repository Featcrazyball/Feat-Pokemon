namespace PokemonPocket;

public class Mewtwo : PokemonMaster
{
    public string? Nickname {get;set;}

    private Mewtwo() { } //For EF Core
    public Mewtwo(string nickname, string ownerId) 
    : base("Mewtwo", "Psychic", 106, 110, 90, 154, 90, 130, ownerId, 70, "Pressure")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}