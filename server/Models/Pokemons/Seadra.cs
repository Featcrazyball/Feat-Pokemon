namespace PokemonPocket;

public class Seadra : PokemonMaster
{
    public string? Nickname {get;set;}

    private Seadra() { } //For EF Core
    public Seadra(string nickname, string ownerId) 
    : base("Seadra", "Water", 55, 65, 95, 95, 45, 85, ownerId, 25, "Poison Point")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}