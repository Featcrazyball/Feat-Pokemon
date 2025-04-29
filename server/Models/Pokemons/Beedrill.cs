namespace PokemonPocket;

public class Beedrill : PokemonMaster
{
    public string? Nickname {get;set;}

    private Beedrill() { } //For EF Core
    public Beedrill(string nickname, string ownerId) 
    : base("Beedrill", "Bug/Poison", 65, 90, 40, 45, 80, 75, ownerId, 20, "Swarm")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return 2*SkillDamage;
    }
}