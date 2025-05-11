using Server;
using PokemonPocket;

namespace Models;

public class Mimic : Skill
{
    private Mimic() { } // For EF Core
    public Mimic(string PokemonId) : base("Mimic", "Normal", 0, 1, 10, 1, 0, 0, "The user copies the target's last move. The move can be used during the battle.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        // Check if target has used a move
        if (target.Lastmove == null)
        {
            await UserSession.SendMessageAsync($"Your {user.Name} used Mimic, but there was nothing to copy!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Mimic, but there was nothing to copy!");
            return;
        }
        
        string moveName = target.Lastmove?.Name ?? "Unknown";
        Skill? CopiedMove = user.ArenaTempSkillGain(moveName.ToLower());
        
        if (CopiedMove == null)
        {
            await UserSession.SendMessageAsync($"Your {user.Name} used Mimic, but it couldn't copy the move!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Mimic, but it couldn't copy the move!");
            return;
        }
        else
        {
            CopiedMove.Mimic = true;

            await UserSession.SendMessageAsync($"Your {user.Name} used Mimic and copied {target.Name}'s {target.Lastmove!.Name}!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Mimic and copied your {target.Name}'s {target.Lastmove!.Name}!");
        }
    }
    
}