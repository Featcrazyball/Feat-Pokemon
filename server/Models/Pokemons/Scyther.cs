namespace PokemonPocket;

public class Scyther : PokemonMaster
{
    public string? Nickname {get;set;}

    private Scyther() { } //For EF Core
    public Scyther(string nickname, string ownerId) 
    : base("Scyther", "Bug/Flying", 70, 110, 80, 55, 80, 105, ownerId, 20, "Swarm")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}