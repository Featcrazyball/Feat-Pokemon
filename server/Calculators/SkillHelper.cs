using Server;
using PokemonPocket;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Models;

public static class SkillHelper
{
    // Check if attack hits based on accuracy
    public static async Task<bool> CheckAccuracy(double accuracy, PokemonMaster user, PokemonMaster target, 
                                                ClientSession userSession, ClientSession targetSession, string skillName)
    {
        if (Random.Shared.NextDouble() >= (accuracy * (CalculateStage(user.AccuracyStage) / CalculateStage(target.EvasionStage))))
        {
            await userSession.SendMessageAsync($"Your {user.Name} used {skillName}, but it missed!");
            await targetSession.SendMessageAsync($"{userSession.Username}'s {user.Name} used {skillName}, but it missed!");
            return false;
        }
        return true;
    }

    // Calculate and apply critical hit
    public static async Task<float> ApplyCriticalHit(float damage, PokemonMaster user, 
                                                    ClientSession userSession, ClientSession targetSession)
    {
        if (Random.Shared.NextDouble() <= user.CritRate)
        {
            await userSession.SendMessageAsync("CRITICAL HIT!");
            await targetSession.SendMessageAsync("CRITICAL HIT!");
            return damage * user.CritDmg;
        }
        return damage;
    }

    // Dig
    public static async Task ProcessDig(PokemonMaster target, PokemonMaster user, 
                                            ClientSession userSession, ClientSession targetSession)
    {
        user.Dig = false;
        user.Underground = false;

        // Remember to make modifier
        user.DigDamage = await FeatCalculateDamage(80, 
            user, 
            target, 
            await GetEffectiveness(userSession, targetSession, "Ground", target.Type?.Split('/') ?? Array.Empty<string>()), 
            userSession, 
            targetSession
        );;

        float damage = user.DigDamage; 
        user.DigDamage = 0;
        
        if (Random.Shared.NextDouble() <= (1 * (CalculateStage(target.AccuracyStage) * CalculateStage(user.EvasionStage))))
        {
            await userSession.SendMessageAsync($"Your {user.Name} used Counter, but it missed!");
            await targetSession.SendMessageAsync($"{userSession.Username}'s {user.Name} used Counter, but it missed!");
        }

        if (target.Substitude)
        {
            if (target.SubstituteHealth <= damage)
            {
                target.Substitude = false;
                target.SubstituteHealth = 0;
                await userSession.SendMessageAsync($"Your {user.Name} used Dig and broke {target.Name}'s Substitute!");
                await targetSession.SendMessageAsync($"{userSession.Username}'s {user.Name} used Dig and broke your {target.Name}'s Substitute!");
            }
            else
            {
                target.SubstituteHealth -= damage;
                await userSession.SendMessageAsync($"Your {user.Name} used Dig on {target.Name}'s Substitute, dealing {damage:F1} damage!");
                await targetSession.SendMessageAsync($"{userSession.Username}'s {user.Name} used Dig on your {target.Name}'s Substitute, dealing {damage:F1} damage!");
            }
        }
        else
        {
            target.Health -= damage;
            await ProcessRage(user, userSession, targetSession);
            if (target.Health < 0) {target.Health = 0;}
            await userSession.SendMessageAsync($"Your {user.Name} used Dig and dealt {damage:F1} damage!");
            await targetSession.SendMessageAsync($"{userSession.Username}'s {user.Name} used Dig and dealt {damage:F1} damage!");
        }

    }

    // Fly
    public static async Task ProcessFly(PokemonMaster target, PokemonMaster user, 
                                            ClientSession userSession, ClientSession targetSession)
    {
        user.Flying = false;
        user.Underground = false;

        float damage = await FeatCalculateDamage(
            90, 
            user, 
            target, 
            await GetEffectiveness(userSession, targetSession, "Flying", target.Type?.Split('/') ?? Array.Empty<string>()), 
            userSession, 
            targetSession
            );
        
        if (Random.Shared.NextDouble() <= (1 * (CalculateStage(target.AccuracyStage) * CalculateStage(user.EvasionStage))))
        {
            await userSession.SendMessageAsync($"Your {user.Name} used Fly, but it missed!");
            await targetSession.SendMessageAsync($"{userSession.Username}'s {user.Name} used Fly, but it missed!");
        }

        if (target.Substitude)
        {
            if (target.SubstituteHealth <= damage)
            {
                target.Substitude = false;
                target.SubstituteHealth = 0;
                await userSession.SendMessageAsync($"Your {user.Name} used Fly and broke {target.Name}'s Substitute!");
                await targetSession.SendMessageAsync($"{userSession.Username}'s {user.Name} used Fly and broke your {target.Name}'s Substitute!");
            }
            else
            {
                target.SubstituteHealth -= damage;
                await userSession.SendMessageAsync($"Your {user.Name} used Fly on {target.Name}'s Substitute, dealing {damage:F1} damage!");
                await targetSession.SendMessageAsync($"{userSession.Username}'s {user.Name} used Fly on your {target.Name}'s Substitute, dealing {damage:F1} damage!");
            }
        }
        else
        {
            target.Health -= damage;
            await ProcessRage(user, userSession, targetSession);
            if (target.Health < 0) {target.Health = 0;}
            await userSession.SendMessageAsync($"Your {user.Name} used Fly and dealt {damage:F1} damage!");
            await targetSession.SendMessageAsync($"{userSession.Username}'s {user.Name} used Fly and dealt {damage:F1} damage!");
        }
    }

    // Type Effectiveness
    public static async Task<float> GetEffectiveness(ClientSession user, ClientSession target, string attackType, params string[] defendTypes)
    {
        if (string.IsNullOrWhiteSpace(attackType)) return 1f;
        if (!TypeChart._chart.TryGetValue(attackType, out var row)) return 1f;

        float multiplier = 1f;
        foreach (var def in defendTypes.Where(d => !string.IsNullOrWhiteSpace(d)))
        {
            if (row.TryGetValue(def, out float m))
            {
                // Immunity (0×) short-circuits everything.
                if (m == 0f) return 0f;
                multiplier *= m;
            }
        }

        switch (multiplier)
        {
            case 0f:
                await user.SendMessageAsync($"Nothing Happens...");
                await target.SendMessageAsync($"Nothing Happens...");
                break;
            case 0.5f:
                await user.SendMessageAsync($"Not very effective...");
                await target.SendMessageAsync($"Not very effective...");
                break;
            case 2f:
                await user.SendMessageAsync($"SUPER EFFECTIVE!");
                await target.SendMessageAsync($"SUPER EFFECTIVE!");
                break;
        }

        return (float)multiplier;
    }

    public static float QuietGetEffectiveness(string attackType, params string[] defendTypes)
    {
        if (string.IsNullOrWhiteSpace(attackType)) return 1f;
        if (!TypeChart._chart.TryGetValue(attackType, out var row)) return 1f;

        float multiplier = 1f;
        foreach (var def in defendTypes.Where(d => !string.IsNullOrWhiteSpace(d)))
        {
            if (row.TryGetValue(def, out float m))
            {
                // Immunity (0×) short-circuits everything.
                if (m == 0f) return 0f;
                multiplier *= m;
            }
        }

        return (float)multiplier;
    }

    public static async Task<float> FeatCalculateDamage(int basePower, PokemonMaster user, PokemonMaster target, float typeEffectiveness, ClientSession UserSession, ClientSession TargetSession) {
        float levelFactor = (2f * user.Level) / 5f + 2f;
        float baseDamage = (levelFactor * basePower * user.Attack) / target.Defense;
        float damage = ((baseDamage / 50f) + 2f) * typeEffectiveness;

        if (Random.Shared.NextDouble() <= user.CritRate)
        {
            await UserSession.SendMessageAsync("CRITICAL HIT!");
            await TargetSession.SendMessageAsync("CRITICAL HIT!");
            return damage * user.CritDmg;
        }
        return target.Reflect ? damage / 2 : damage;
    }

    public static async Task<float> FeatCalculateSpecialDamage(int basePower, PokemonMaster user, PokemonMaster target, float typeEffectiveness, ClientSession UserSession, ClientSession TargetSession) {
        float levelFactor = (2f * user.Level) / 5f + 2f;
        float baseDamage = (levelFactor * basePower * user.SpecialAttack) / target.SpecialDefense;
        float damage = ((baseDamage / 50f) + 2f) * typeEffectiveness;

        if (Random.Shared.NextDouble() <= user.CritRate)
        {
            await UserSession.SendMessageAsync("CRITICAL HIT!");
            await TargetSession.SendMessageAsync("CRITICAL HIT!");
            return damage * user.CritDmg;
        }
        return target.LightScreen ? damage / 2 : damage;
    }

    public static double CalculateStage(int stage)
    {
        switch (stage)
        {
            case -6: return 0.25;
            case -5: return 2.0/7.0;
            case -4: return 1.0/3.0;
            case -3: return 0.4;
            case -2: return 0.5;
            case -1: return 2.0/3.0;
            case 0: return 1;
            case 1: return 1.5;
            case 2: return 2;
            case 3: return 2.5;
            case 4: return 3;
            case 5: return 3.5;
            case 6: return 4;
            default:
                throw new ArgumentOutOfRangeException("Stages must be between -6 and +6. Please contact an admin");
        }
    }

    public static bool CheckPhysical(string s)
    {
        string[] physical = { "Normal", "Fighting", "Flying", "Poison", "Ground", "Rock", "Bug", "Steel" };
        foreach (string type in s.Split('/'))
        {
            if (physical.Contains(type)) return true;
        }
        return false;
    }

    public static async Task<bool> ProcessConfusion(PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        if (Random.Shared.NextDouble() <= 0.5)
        {
            if (Random.Shared.NextDouble() <= (1 * (CalculateStage(user.AccuracyStage) / CalculateStage(user.EvasionStage))))
            {
                await UserSession.SendMessageAsync($"Your {user.Name} hurt itself in confusion, but it missed!");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} hurt itself in confusion, but it missed!");
                return false;
            }

            float damage = await FeatCalculateDamage(40, user, user, 1, UserSession, TargetSession); 

            if (user.Substitude)
            {
                if (user.SubstituteHealth <= damage)
                {
                    user.Substitude = false;
                    user.SubstituteHealth = 0;
                    await UserSession.SendMessageAsync($"Your {user.Name} broke its own Substitute!");
                    await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} broke its own Substitute!");
                }
                else
                {
                    user.SubstituteHealth -= damage;
                    await UserSession.SendMessageAsync($"Your {user.Name} hurt itself in confusion, dealing {damage:F1} damage to its Substitute!");
                    await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} hurt itself in confusion, dealing {damage:F1} damage to its Substitute!");
                }
            } 
            else 
            {
                user.Health -= damage;
                await UserSession.SendMessageAsync($"Your {user.Name} hurt itself in confusion, dealing {damage:F1} damage!");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} hurt itself in confusion, dealing {damage:F1} damage!");
            } 

            return false;
        } else {
            return true;
        }
    }

    public static async Task ProcessRage(PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        if (user.RageActive == true && user.Lastmove!.Name == "Rage" && user.Health > 0)
        {
            user.AttackStage += 1;
            user.Attack = (float)(user.MaxAttack * CalculateStage(user.AttackStage));
            if (user.Burning) {user.Attack *= (float)0.5;}
            
            await UserSession.SendMessageAsync($"Your {user.Name} is enraged and its Attack rose by 1 Stage!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} is enraged and its Attack rose by 1 Stage!");
        }
    }

    public static async Task MoveUpdater(Skill skill, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        user.Lastmove = skill;
        if (user.Firstmove == null) 
            user.Firstmove = skill;

        if (user.RageActive && skill.Name != "Rage")
        {
            user.RageActive = false;

            await UserSession.SendMessageAsync($"Your {user.Name} stopped using Rage.");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} stopped using Rage.");
        }
    }
}
