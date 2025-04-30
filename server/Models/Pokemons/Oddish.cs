namespace PokemonPocket;

public class Oddish : PokemonMaster
{
    public string? Nickname {get;set;}

    private Oddish() { } //For EF Core
    public Oddish(string nickname, string ownerId) 
    : base("Oddish", "Grass/Poison", 45, 50, 55, 75, 65, 30, ownerId, 10, "Chlorophyll")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}