namespace PokemonPocket;

public class Arbok : PokemonMaster
{
    public string? Nickname {get;set;}

    private Arbok() { } //For EF Core
    public Arbok(string nickname, string ownerId) 
    : base("Arbok", "Poison", 60, 95, 69, 65, 79, 80, ownerId, 25, "Bite")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}