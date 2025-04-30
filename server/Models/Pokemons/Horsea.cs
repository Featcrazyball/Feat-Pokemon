namespace PokemonPocket;

public class Horsea : PokemonMaster
{
    public string? Nickname {get;set;}

    private Horsea() { } //For EF Core
    public Horsea(string nickname, string ownerId) 
    : base("Horsea", "Water", 30, 40, 70, 70, 25, 60, ownerId, 10, "Swift Swim")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}