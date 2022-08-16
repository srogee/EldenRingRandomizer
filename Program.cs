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

            if (!Directory.Exists(preferences.GameInstallDirectory))
            {
                Console.WriteLine("Game install directory is not set or does not exist");
                return;
            }

            string regulationInPath = Path.Combine(preferences.GameInstallDirectory, "regulation.bin.bak");
            string regulationOutPath = Path.Combine(preferences.GameInstallDirectory, "regulation.bin");

            if (!File.Exists(regulationInPath))
            {
                Console.WriteLine("regulation.bin.bak file not found in game install directory");
                return;
            }

            Console.WriteLine("Found regulation.bin.bak file in game install directory");
            Console.WriteLine();

#if GenerateCode
            ParamClassGenerator.Generate();
#else
            ItemRandomizerParams randomizerParams;

            if (ConsolePrompt.Bool("Edit randomizer params"))
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
                var randomizerParamsJson = ConsolePrompt.String("Enter randomizer params JSON string");
                randomizerParams = JSON.Parse<ItemRandomizerParams>(randomizerParamsJson);
            }

            Console.WriteLine();
            Console.WriteLine("Running randomizer with the following params:");
            randomizerParams.PrettyPrint();

            var shareableParamsJson = JSON.Stringify(randomizerParams);
            
            var randomizer = new ItemRandomizer(randomizerParams, regulationInPath, regulationOutPath);
            randomizer.OnProgressChanged += DrawProgressBar;
            
            Console.WriteLine();
            randomizer.Run();
            Console.WriteLine();

            Console.WriteLine();
            Console.WriteLine("Share this JSON string with others:");
            Console.WriteLine(shareableParamsJson);
#endif
        }

        private static void DrawProgressBar(float percent, string message)
        {
            Console.Write($"\r{message} ({(percent * 100).ToString("F0")}%)                     ");
        }
    }
}
