namespace PokemonPocket;

public class Slowpoke : PokemonMaster
{
    public string? Nickname {get;set;}

    private Slowpoke() { } //For EF Core
    public Slowpoke(string nickname, string ownerId) 
    : base("Slowpoke", "Water/Psychic", 90, 65, 65, 40, 40, 15, ownerId, 20, "Oblivious")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}