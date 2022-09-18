using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EldenRingItemRandomizer
{
    internal class ItemRandomizerParams
    {
        public string Version = Program.Version;
        public int Seed = -1; // Invalid
        public float WeaponBaseDamageMultiplier = 1.5f;
        public float WeaponScalingMultiplier = 10.0f;
        public int GreatRunesRequired = 7;
        public bool GreatRunesFromBossLegend = true;
        public bool GreatRunesFromBossGreatEnemy = true;
        public bool GreatRunesFromBossField = true;

        public ItemRandomizerParams() { }

        public void PrettyPrint()
        {
            Console.WriteLine($"General");
            Console.WriteLine($"\tSeed: {Seed}");

            Console.WriteLine($"Weapons");
            Console.WriteLine($"\tBase Damage Multiplier: {WeaponBaseDamageMultiplier.ToString("F2")}x");
            Console.WriteLine($"\tScaling Multiplier: {WeaponScalingMultiplier.ToString("F2")}x");

            Console.WriteLine($"Bosses");
            Console.WriteLine($"\tGreat Runes Required: {GreatRunesRequired}");
            Console.WriteLine($"\tGreat Runes Drop From Demigods/Legends: {BoolToYesNo(GreatRunesFromBossLegend)}");
            Console.WriteLine($"\tGreat Runes Drop From (some) Great Enemies: {BoolToYesNo(GreatRunesFromBossGreatEnemy)}");
            Console.WriteLine($"\tGreat Runes Drop From (some) Field Bosses: {BoolToYesNo(GreatRunesFromBossField)}");
        }

        private static string BoolToYesNo(bool value)
        {
            return value ? "Yes" : "No";
        }
    }
}
