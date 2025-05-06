using Server;
using PokemonPocket;

namespace Models;

public class Bind : Skill
{
    private Bind() { } // For EF Core
    public Bind(string PokemonId) : base("Bind", "Normal", 15, 0.85, 20, 1, 0, 0, "The user wraps its body around the target and squeezes it for two to five turns.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, float Modifier, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        if (target.BindActive) {await UserSession.SendMessageAsync($"Opponent already being bind, thus binding fails"); return;}
        
        if (Random.Shared.NextDouble() > Accuracy) {await UserSession.SendMessageAsync($"Your {user.Name} used Bind, but it missed!"); return;}

        bool crit = false;
        if (Random.Shared.NextDouble() > user.CritRate) {crit = true;}
        if (crit) 
        {
            await UserSession.SendMessageAsync("CRITICAL HIT!"); 
            await TargetSession.SendMessageAsync("CRITICAL HIT!");
        }

        int turns;
        float chance = Random.Shared.Next(0, 100);
        if (chance > 87.5) turns = 5;
        else if (chance > 75) turns = 4;
        else if (chance > 37.5) turns = 3;
        else turns = 2;

        float damage = ((user.Level * 2 / 5 + 2) * BasePower * (crit ? user.CritDmg : 1) * user.Attack / target.Defense / 50 + 2) * Modifier;

        target.BindDamage = damage;
        target.BindTurns = turns;
        target.BindActive = true;

        await UserSession.SendMessageAsync($"Your {user.Name} used Bind and is now squeezing the target!");
        await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Bind and is now squeezing the target!");
    }
}