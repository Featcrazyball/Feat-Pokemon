using Server;
using PokemonPocket;
using FeatCalculator;

namespace Models;

public class AcidArmor : Skill
{
    private AcidArmor() { } // For EF Core
    public AcidArmor(string PokemonId) : base("Acid Armor", "Poison", 0, -1, 20, 1, 0, 0, "The user alters its cellular structure to liquefy itself, raising its Defense.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, float Modifier, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        for (int i = 0; i < 2; i++)
            if (user.DefenseStage >= 6) {break;}
            user.Defense = (float)(user.MaxDefense * Calculator.CalculateStage(user.DefenseStage));
            user.DefenseStage += 1;

        await UserSession.SendMessageAsync($"Your {user.Name} used Acid Armor, increasing its Defense to {user.Defense}.");
        await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Acid Armor, increasing its Defense to {user.Defense}.");
    }
}