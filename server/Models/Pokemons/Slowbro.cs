namespace PokemonPocket;

public class Slowbro : PokemonMaster
{
    public string? Nickname {get;set;}

    private Slowbro() { } //For EF Core
    public Slowbro(string nickname, string ownerId) 
    : base("Slowbro", "Water/Psychic", 95, 75, 110, 100, 80, 30, ownerId, 36, "Oblivious")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}