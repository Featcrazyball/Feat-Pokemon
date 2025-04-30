namespace PokemonPocket;

public class Aerodactyl : PokemonMaster
{
    public string? Nickname {get;set;}

    private Aerodactyl() { } //For EF Core
    public Aerodactyl(string nickname, string ownerId) 
    : base("Aerodactyl", "Rock/Flying", 80, 105, 65, 60, 75, 130, ownerId, 20, "Pressure")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}