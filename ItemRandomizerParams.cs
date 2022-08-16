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
        public int Seed = 0;
        public float WeaponBaseDamageMultiplier = 1.0f;
        public float WeaponScalingMultiplier = 1.0f;

        public ItemRandomizerParams() { }

        public void PrettyPrint()
        {
            Console.WriteLine($"General");
            Console.WriteLine($"\tSeed: {Seed}");
            Console.WriteLine($"Weapons");
            Console.WriteLine($"\tBase Damage Multiplier: {WeaponBaseDamageMultiplier.ToString("F2")}x");
            Console.WriteLine($"\tScaling Multiplier: {WeaponScalingMultiplier.ToString("F2")}x");
        }
    }
}
