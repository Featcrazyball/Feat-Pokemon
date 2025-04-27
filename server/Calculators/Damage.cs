namespace Damage
{
    public class Damage
    {
        public float FeatCalculateDamage(int basePower, float attack, float defense, float level, float typeEffectiveness) {
            float damage = ((2 * level / 5 + 2) * basePower * attack / defense / 50 + 2) * typeEffectiveness;
            return damage;
        }

        public float FeatCalculateSpecialDamage(int basePower, float specialAttack, float specialDefense, float level, float typeEffectiveness) {
            float damage = ((2 * level / 5 + 2) * basePower * specialAttack / specialDefense / 50 + 2) * typeEffectiveness;
            return damage;
        }
    }
}