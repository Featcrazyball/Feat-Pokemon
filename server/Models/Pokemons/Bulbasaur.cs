namespace PokemonPocket;

public class Bulbasaur : PokemonMaster
{
    public string? Nickname {get;set;}

    private Bulbasaur() { } //For EF Core
    public Bulbasaur(string nickname, string ownerId) 
    : base("Bulbasaur", "Grass/Poison", 45, 49, 49, 65, 65, 45, ownerId, 10, "Water Burst")
    {
        Nickname = nickname;
    }

    // Ask Teacher
    public override float calculateDamage(float SkillDamage) {
        return 2*SkillDamage;
    }
}