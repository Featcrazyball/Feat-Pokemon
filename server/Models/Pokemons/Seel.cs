namespace PokemonPocket;

public class Seel : PokemonMaster
{
    public string? Nickname {get;set;}

    private Seel() { } //For EF Core
    public Seel(string nickname, string ownerId) 
    : base("Seel", "Water", 65, 45, 55, 45, 70, 45, ownerId, 15, "Thick Fat")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}