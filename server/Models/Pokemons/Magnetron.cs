namespace PokemonPocket;

public class Magnetron : PokemonMaster
{
    public string? Nickname {get;set;}

    private Magnetron() { } //For EF Core
    public Magnetron(string nickname, string ownerId) 
    : base("Magnetron", "Electric/Steel", 50, 60, 95, 120, 70, 70, ownerId, 20, "Magnet Pull")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}