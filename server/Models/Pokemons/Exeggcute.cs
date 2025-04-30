namespace PokemonPocket;

public class Exeggcute : PokemonMaster
{
    public string? Nickname {get;set;}

    private Exeggcute() { } //For EF Core
    public Exeggcute(string nickname, string ownerId) 
    : base("Exeggcute", "Grass/Psychic", 60, 40, 80, 60, 45, 40, ownerId, 20, "Chlorophyll")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}