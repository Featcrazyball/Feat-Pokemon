namespace PokemonPocket;

public class Kadabrah : PokemonMaster
{
    public string? Nickname {get;set;}

    private Kadabrah() { } //For EF Core
    public Kadabrah(string nickname, string ownerId) 
    : base("Kadabrah", "Psychic", 40, 35, 30, 120, 70, 105, ownerId, 50, "Synchronize")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}