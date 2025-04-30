namespace PokemonPocket;

public class Bellsprout : PokemonMaster
{
    public string? Nickname {get;set;}

    private Bellsprout() { } //For EF Core
    public Bellsprout(string nickname, string ownerId) 
    : base("Bellsprout", "Grass/Poison", 50, 75, 35, 70, 30, 40, ownerId, 10, "Chlorophyll")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}