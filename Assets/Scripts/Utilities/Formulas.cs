using Game.Behaviors;
using System;
using UnityEngine;

namespace Assets.Scripts.Utilities
{
    public static class Formulas
    {
        private static LogManager log => GameManager.instance.logManager;


        // Method to calculate StatGrowth
        public static float StatGrowth(int level)
        {
            return Mathf.Round(100f * (level / 100.0f) * Random.Float(0.4f, 0.8f));
        }

        public static ActorStats RandomStats(int level)
        {
            ActorStats stats = new ActorStats()
            {
                Level = level,
                Strength = StatGrowth(level),
                Endurance = StatGrowth(level),
                Dexterity = StatGrowth(level),
                Speed = StatGrowth(level),
                Luck = StatGrowth(level),
            };

            stats.MaxHP = level * 3 + StatGrowth(level);
            stats.HP = stats.MaxHP;

            return stats;
        }

        public static float LuckModifier(ActorStats stats)
        {
            var multiplier = stats.Level * 0.01f;
            return Random.Float(1, 1f + stats.Luck * multiplier);
        }

        public static float Accuracy(ActorStats stats)
        {
            var baseAccuracy = 50f + ((stats.Level - 1) / 99.0f) * 50f;
            var multiplier = 1.0f;
            var dex = (stats.Dexterity * multiplier / 100f * 100f);
            var lck = LuckModifier(stats);
            return Mathf.Round(baseAccuracy + dex + lck);
        }

        public static float Evasion(ActorStats stats)
        {
            var multiplier = 1.0f;
            var spd = (stats.Speed * multiplier / 100f * 100f);
            var lck = LuckModifier(stats);
            var armor = 10;
            var armorModifier = (armor * 0.1666f / 100f * 100f);

            return Mathf.Round(spd + lck - armorModifier);
        }

        // Method to calculate Offense
        public static float Offense(ActorStats stats)
        {
            var weapon = 10;
            var atk = ((stats.Strength * (1f + weapon / 100f)) * 0.65f / 100f * 100f);
            var lck = LuckModifier(stats);

            return Mathf.Round(atk + lck);
        }

        // Method to calculate Defense
        public static float Defense(ActorStats stats)
        {
            var armor = 10;
            var def = ((stats.Endurance * (1f + armor / 100f)) * 0.25f / 100f * 100f);
            var lck = LuckModifier(stats);

            return Mathf.Round(def + lck);
        }

        

        public static bool IsHit(ActorStats attacker, ActorStats defender)
        {
            var accuracy = Accuracy(attacker);
            var evasion = Evasion(defender);   
            var d100 = Random.Int(1, 100);
            var isHit = accuracy - evasion >= d100;

            var msg
                = $@"Accuracy({accuracy}) - Evasion({evasion}) "
                + $@"{(isHit ? ">" : "<")} "
                + $@"1d100({d100}) => {(isHit ? "Hit" : "Miss")}";
            log.info(msg);

            return isHit;
        }

        public static int CalculateDamage(ActorStats attacker, ActorStats defender)
        {
            var offense = Offense(attacker);
            var defense = Defense(defender);

            return (int)Math.Round(offense - defense);
        }

    }
}
