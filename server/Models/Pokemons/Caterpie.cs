namespace PokemonPocket;

public class Caterpie : PokemonMaster
{
    public string? Nickname {get;set;}

    private Caterpie() { } //For EF Core
    public Caterpie(string nickname, string ownerId) 
    : base("Caterpie", "Bug", 45, 30, 35, 20, 20, 45, ownerId,  10, "Shield Dust")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return 2*SkillDamage;
    }
}