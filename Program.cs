//#define GenerateCode

using SoulsFormats;
using StronglyTypedParams;
using System;
using System.Collections.Generic;
using System.IO;

namespace EldenRingItemRandomizer
{
    partial class Program
    {
        static void Main(string[] args)
        {
            Preferences preferences = JSON.ParseFile<Preferences>("preferences.json");
#if GenerateCode
            ParamClassGenerator.Generate();
#else
            ItemRandomizerParams randomizerParams;

            if (ConsolePrompt.Bool("Enter params manually"))
            {
                int seed = 0;
                if (ConsolePrompt.Bool("Generate random seed"))
                {
                    seed = new Random().Next();
                }
                else
                {
                    seed = ConsolePrompt.Int("Seed");
                }
                randomizerParams = new ItemRandomizerParams()
                {
                    Seed = seed,
                    WeaponBaseDamageMultiplier = ConsolePrompt.Float("Weapon Base Damage Multiplier"),
                    WeaponScalingMultiplier = ConsolePrompt.Float("Weapon Scaling Multiplier")
                };
            }
            else
            {
                var randomizerParamsJson = ConsolePrompt.String("Enter the randomizer params in JSON format");
                randomizerParams = JSON.Parse<ItemRandomizerParams>(randomizerParamsJson);
            }

            randomizerParams.PrettyPrint();

            var shareableParamsJson = JSON.Stringify(randomizerParams);
            
            var randomizer = new ItemRandomizer(randomizerParams);
            randomizer.OnProgressChanged += (percent, message) => Console.WriteLine($"{(percent * 100).ToString("F0")}% - {message}");
            randomizer.Run();

            Console.WriteLine("Shareable Params JSON: ");
            Console.WriteLine(shareableParamsJson);
#endif
        }
    }
}
