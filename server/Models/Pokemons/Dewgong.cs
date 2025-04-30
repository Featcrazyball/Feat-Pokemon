namespace PokemonPocket;

public class Dewgong : PokemonMaster
{
    public string? Nickname {get;set;}

    private Dewgong() { } //For EF Core
    public Dewgong(string nickname, string ownerId) 
    : base("Dewgong", "Water/Ice", 90, 70, 80, 70, 95, 70, ownerId, 30, "Thick Fat")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}