namespace PokemonPocket;

public class Abra : PokemonMaster
{
    public string? Nickname {get;set;}

    private Abra() { } //For EF Core
    public Abra(string nickname, string ownerId) 
    : base("Abra", "Psychic", 25, 20, 15, 105, 55, 90, ownerId, 10, "Synchronize")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}