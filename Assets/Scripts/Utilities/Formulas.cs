using Assets.Scripts.Models;
using System;

namespace Assets.Scripts.Utilities
{
    public static class Formulas
    {

        public static float CalculateLevelModifier(int level)
        {
            return 50f + ((level - 1) / 99.0f) * 50f;
        }

        public static float CalculateLuckModifier(float luck)
        {
            return Random.Float(1, 1f + luck * 0.05f);
        }

        public static float CalculateHitChance(float accuracy, float luck, int level)
        {
            return 
                CalculateLevelModifier(level) 
              + (accuracy * 0.65f / 100f * 100f) 
              + CalculateLuckModifier(luck);
        }

        public static float CalculateEvadeChance(float evasion, float luck, float armor, int level)
        {
            return 
                (evasion * 0.25f / 100f * 100f) 
              + CalculateLuckModifier(luck) 
              - (armor * 0.05f / 100f * 100f);
        }

        // Method to calculate Attack
        public static float CalculateAttack(float strength, float weapon, float luck)
        {
            return 
                ((strength * (1f + weapon / 100f)) * 0.65f / 100f * 100f)
              + CalculateLuckModifier(luck);
        }

        // Method to calculate Defense
        public static float CalculateDefense(float endurance, float armor, float luck)
        {
            return 
                ((endurance * (1f + armor / 100f)) * 0.25f / 100f * 100f) 
              + CalculateLuckModifier(luck);
        }

        // Method to calculate StatGrowth
        public static float CalculateStatGrowth(int level)
        {
            return 100f * (level / 100.0f) * Random.Float(0.4f, 0.8f);
        }

        public static bool IsHit(ActorStats attacker, ActorStats defender)
        {
            return
                CalculateHitChance(attacker.Accuracy, attacker.Luck, attacker.Level)
              - CalculateEvadeChance(defender.Evasion, defender.Luck, 1, defender.Level)
                > Random.Float(0, 100);
        }

        public static int CalculateDamage(ActorStats attacker, ActorStats defender)
        {

            var attack = CalculateAttack(attacker.Strength, 1, attacker.Luck);
            var defense = CalculateDefense(defender.Endurance, 1, defender.Luck);

            return (int)Math.Round(attack - defense);
        }

    }
}
