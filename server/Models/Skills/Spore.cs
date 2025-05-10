using Server;
using PokemonPocket;

namespace Models;

public class Spore : Skill
{
    private Spore() { } // For EF Core
    public Spore(string PokemonId) : base("Spore", "Grass", 0, 1, 15, 1, 0, 0, "The user scatters bursts of spores that induce sleep.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        // Check if target already has a status condition
        if (target.Sleeping)
        {
            await UserSession.SendMessageAsync($"Your {user.Name} used Spore, but {target.Name} is already sleeping!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Spore, but your {target.Name} is already sleeping!");
            return;
        }

        // Check if protected by substitute
        if (target.Substitude)
        {
            await UserSession.SendMessageAsync($"Your {user.Name} used Spore, but {target.Name}'s substitute blocked it!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Spore, but your {target.Name}'s substitute blocked it!");
            return;
        }
        
        // Put target to sleep
        target.Sleeping = true;
        
        // Calculate number of turns (1-4 in Gen 1)
        target.SleepTurns = Random.Shared.Next(1, 4);
        
        await UserSession.SendMessageAsync($"Your {user.Name} used Spore! {target.Name} fell asleep for {target.SleepTurns}!");
        await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Spore! Your {target.Name} fell asleep for {target.SleepTurns}!");
    }
}