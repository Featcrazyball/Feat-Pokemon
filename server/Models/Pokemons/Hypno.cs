namespace PokemonPocket;

public class Hypno : PokemonMaster
{
    public string? Nickname {get;set;}

    private Hypno() { } //For EF Core
    public Hypno(string nickname, string ownerId) 
    : base("Hypno", "Psychic", 85, 73, 70, 73, 115, 67, ownerId, 30, "Insomnia")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}