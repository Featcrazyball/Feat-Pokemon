namespace PokemonPocket;

public class Gengar : PokemonMaster
{
    public string? Nickname {get;set;}

    private Gengar() { } //For EF Core
    public Gengar(string nickname, string ownerId) 
    : base("Gengar", "Ghost/Poison", 60, 65, 60, 130, 75, 110, ownerId, 25, "Cursed Body")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}