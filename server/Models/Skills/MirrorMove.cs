using Server;
using PokemonPocket;

namespace Models;

public class MirrorMove : Skill
{
    private MirrorMove() { } // For EF Core
    public MirrorMove(string PokemonId) : base("Mirror Move", "Flying", 0, -1, 20, 1, 0, 0, "The user reflects the last move used by the target back at it with the same power.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        if (target.Lastmove == null) {
            await UserSession.SendMessageAsync($"Your {user.Name} used Mirror Move, but the target has not used a move yet!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Mirror Move, but the target has not used a move yet!");
        } 
        else if (target.Lastmove.Name == "Mirror Move") {
            await UserSession.SendMessageAsync($"Your {user.Name} used Mirror Move, but since {TargetSession.Username}'s {target.Name}'s last move was Mirror Move, it fails!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Mirror Move, but since your {target.Name}'s last move was Mirror Move, it fails!");
        }
        else
        {
            await UserSession.SendMessageAsync($"Your {user.Name} used Mirror Move!");
            await UserSession.SendMessageAsync($"Your {user.Name} will now perform the last move used by {TargetSession.Username}'s {target.Name}!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Mirror Move!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} will now perform the last move used by your {target.Name}!");

            await target.Lastmove.SkillEfect(target, user, UserSession, TargetSession);
        }

    }
}