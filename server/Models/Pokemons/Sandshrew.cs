namespace PokemonPocket;

public class Sandshrew : PokemonMaster
{
    public string? Nickname {get;set;}

    private Sandshrew() { } //For EF Core
    public Sandshrew(string nickname, string ownerId) 
    : base("Sandshrew", "Ground", 50, 75, 85, 20, 30, 40, ownerId, 25, "Scratch")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}