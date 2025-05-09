using Server;
using PokemonPocket;

namespace Models;

public class Psychic : Skill
{
    private Psychic() { } // For EF Core
    public Psychic(string PokemonId) : base("Psychic", "Psychic", 90, 1, 10, 1, 0, 0, "The target is hit by a strong telekinetic force. It may also reduce the target's Sp. Def stat.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        // Accuracy check
        if (await SkillHelper.CheckAccuracy(Accuracy, user, target, UserSession, TargetSession, skillName: "Psychic") == false)
            return;

        // Damage calculation
        float damage = await SkillHelper.FeatCalculateSpecialDamage(
            BasePower, 
            user, 
            target, 
            await SkillHelper.GetEffectiveness(UserSession, TargetSession, "Psychic", target.Type?.Split('/') ?? Array.Empty<string>()),  
            UserSession, 
            TargetSession
        );
        
        bool decrease = false;
        bool Mist = false;

        // Substitute handling
        if (target.Substitude)
        {
            if (target.SubstituteHealth <= damage)
            {
                target.Substitude = false;
                target.SubstituteHealth = 0;
                
                if (Random.Shared.NextDouble() <= 0.10)
                {
                    for (int i = 0; i < 1; i++)
                    {
                        if (target.SpecialDefenseStage >= 6) {break;}
                        if (target.Mist) {Mist = true; break;}
                        decrease = true;
                        target.SpecialDefenseStage -= 1;
                        target.SpecialDefense = target.MaxSpecialDefense * (float)SkillHelper.CalculateStage(target.SpecialDefenseStage); 
                    }
                }

                if (Mist)
                {
                    await UserSession.SendMessageAsync($"Your {user.Name} used Psychic and broke {target.Name}'s Substitute! But {target.Name}'s Mist prevented its Special Defense from falling!");
                    await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Psychic and broke your {target.Name}'s Substitute! But your {target.Name}'s Mist prevented its Special Defense from falling!");
                }
                else if (decrease)
                {
                    await UserSession.SendMessageAsync($"Your {user.Name} used Psychic and broke {target.Name}'s Substitute! {target.Name}'s Special Defense fell by 1 Stage!");
                    await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Psychic and broke your {target.Name}'s Substitute! Your {target.Name}'s Special Defense fell by 1 Stage!");
                }
                else
                {
                    await UserSession.SendMessageAsync($"Your {user.Name} used Psychic and broke {target.Name}'s Substitute!");
                    await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Psychic and broke your {target.Name}'s Substitute!");
                }
            }
            else
            {
                target.SubstituteHealth -= damage;
                
                await UserSession.SendMessageAsync($"Your {user.Name} used Psychic on {target.Name}'s Substitute, dealing {damage:F1} damage.");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Psychic on your {target.Name}'s Substitute, dealing {damage:F1} damage.");
                
                if (target.SubstituteHealth < 0) target.SubstituteHealth = 0;
            }
        }
        else
        {
            target.Health -= damage;
            await SkillHelper.ProcessRage(target, TargetSession, UserSession);
            
            if (Random.Shared.NextDouble() <= 0.10)
            {
                for (int i = 0; i < 1; i++)
                {
                    if (target.SpecialDefenseStage >= 6) {break;}
                    if (target.Mist) {Mist = true; break;}
                    decrease = true;
                    target.SpecialDefenseStage -= 1;
                    target.SpecialDefense = target.MaxSpecialDefense * (float)SkillHelper.CalculateStage(target.SpecialDefenseStage); 
                }
            }

            if (Mist)
            {
                await UserSession.SendMessageAsync($"Your {user.Name} used Psychic on {target.Name}, dealing {damage:F1} damage! But {target.Name}'s Mist prevented its Special Defense from falling!");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Psychic on your {target.Name}, dealing {damage:F1} damage! But your {target.Name}'s Mist prevented its Special Defense from falling!");
            }
            else if (decrease)
            {
                await UserSession.SendMessageAsync($"Your {user.Name} used Psychic on {target.Name}, dealing {damage:F1} damage! {target.Name}'s Special Defense fell by 1 Stage!");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Psychic on your {target.Name}, dealing {damage:F1} damage! Your {target.Name}'s Special Defense fell by 1 Stage!");
            }
            else
            {
                await UserSession.SendMessageAsync($"Your {user.Name} used Psychic on {target.Name}, dealing {damage:F1} damage!");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Psychic on your {target.Name}, dealing {damage:F1} damage!");
            }
        }
    }
}