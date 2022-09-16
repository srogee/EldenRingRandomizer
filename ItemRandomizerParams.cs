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

        public ItemRandomizerParams() { }

        public void PrettyPrint()
        {
            Console.WriteLine($"General");
            Console.WriteLine($"\tSeed: {Seed}");
            Console.WriteLine($"\tGreat Runes Required: {GreatRunesRequired}");
            Console.WriteLine($"Weapons");
            Console.WriteLine($"\tBase Damage Multiplier: {WeaponBaseDamageMultiplier.ToString("F2")}x");
            Console.WriteLine($"\tScaling Multiplier: {WeaponScalingMultiplier.ToString("F2")}x");
        }
    }
}
