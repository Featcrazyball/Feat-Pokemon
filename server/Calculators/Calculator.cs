namespace FeatCalculator;

public class Calculator
{
    public static float FeatCalculateDamage(int basePower, float attack, float defense, float level, float typeEffectiveness) {
        float damage = ((2 * level / 5 + 2) * basePower * attack / defense / 50 + 2) * typeEffectiveness;
        return damage;
    }

    public static float FeatCalculateSpecialDamage(int basePower, float specialAttack, float specialDefense, float level, float typeEffectiveness) {
        float damage = ((2 * level / 5 + 2) * basePower * specialAttack / specialDefense / 50 + 2) * typeEffectiveness;
        return damage;
    }

    public static double CalculateStage(int stage)
    {
        switch (stage)
        {
            case -6: return 0.25;
            case -5: return 2/7;
            case -4: return 1/3;
            case -3: return 0.4;
            case -2: return 0.5;
            case -1: return 2/3;
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
}
