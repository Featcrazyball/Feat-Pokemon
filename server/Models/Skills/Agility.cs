using Server;
using PokemonPocket;
using FeatCalculator;

namespace Models;

public class Agility : Skill
{
    private Agility() { } // For EF Core
    public Agility(string PokemonId) : base("Agility", "Psychic", 0, -1, 30, 1, 0, 0, "The user alters its cellular structure to liquefy itself, raising its Defense.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, float Modifier, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        for (int i = 0; i < 2; i++)
            if (user.SpeedStage >= 6) {break;}
            user.Speed = (float)(user.MaxSpeed * Calculator.CalculateStage(user.SpeedStage));
            user.SpeedStage += 1;

        await UserSession.SendMessageAsync($"Your {user.Name} used Agility, increasing its Speed to {user.Speed}.");
        await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Agility, increasing its Speed to {user.Speed}.");
    }
}