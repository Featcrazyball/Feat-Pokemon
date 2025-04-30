namespace PokemonPocket;

public class Starmie : PokemonMaster
{
    public string? Nickname {get;set;}

    private Starmie() { } //For EF Core
    public Starmie(string nickname, string ownerId) 
    : base("Starmie", "Water/Psychic", 60, 75, 85, 100, 85, 115, ownerId, 30, "Illuminate")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}