namespace PokemonPocket;

public class Golbat : PokemonMaster
{
    public string? Nickname {get;set;}

    private Golbat() { } //For EF Core
    public Golbat(string nickname, string ownerId) 
    : base("Golbat", "Poison/Flying", 75, 80, 70, 65, 75, 90, ownerId, 20, "Inner Focus")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}