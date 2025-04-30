namespace PokemonPocket;

public class Electrode : PokemonMaster
{
    public string? Nickname {get;set;}

    private Electrode() { } //For EF Core
    public Electrode(string nickname, string ownerId) 
    : base("Electrode", "Electric", 60, 50, 70, 80, 80, 150, ownerId, 26, "Static")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}