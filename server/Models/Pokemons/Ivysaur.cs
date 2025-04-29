namespace PokemonPocket;

public class Ivysaur : PokemonMaster
{
    public string? Nickname {get;set;}

    private Ivysaur() { } //For EF Core
    public Ivysaur(string nickname, string ownerId) 
    : base("Ivysaur", "Grass/Poison", 60, 62, 63, 80, 80, 60, ownerId, 20, "Water Burst")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return 2*SkillDamage;
    }
}