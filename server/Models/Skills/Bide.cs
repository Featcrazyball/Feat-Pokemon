using Server;
using PokemonPocket;
using FeatCalculator;

namespace Models;

public class Bide : Skill
{
    private Bide() { } // For EF Core
    public Bide(string PokemonId) : base("Bide", "Normal", 0, -1, 20, 1, 0, 0, "The user endures attacks for two turns and then strikes back double the damage taken.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, float Modifier, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        if (user.BideActive) {await UserSession.SendMessageAsync($"Your {user.Name} is already using Bide!"); return;}
        
        user.BideDamage = 0;
        user.BideTurns = 2;
        user.BideActive = true;

        await UserSession.SendMessageAsync($"Your {user.Name} used Bide and is now waiting to strike back!");
        await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Bide and is now waiting to strike back!");
    }
}