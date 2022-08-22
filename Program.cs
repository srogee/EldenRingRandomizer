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
        public static string Version = "1.1";

        static void Main(string[] args)
        {
            Console.WriteLine($"Elden Ring Item Randomizer v{Version}");
            Console.WriteLine();

            Preferences preferences = JSON.ParseFile<Preferences>("preferences.json");

            if (!Directory.Exists(preferences.GameInstallDirectory))
            {
                Console.WriteLine("Game install directory is not set or does not exist. Set it in preferences.json.");
                return;
            }

            string regulationInPath = Path.Combine(preferences.GameInstallDirectory, "regulation.bin.bak");
            string regulationOutPath = Path.Combine(preferences.GameInstallDirectory, "regulation.bin");

            if (!File.Exists(regulationInPath))
            {
                Console.WriteLine("regulation.bin.bak file not found in game install directory. Copy the vanilla regulation.bin and rename it to regulation.bin.bak.");
                return;
            }

            Console.WriteLine("Found regulation.bin.bak file in game install directory");
            Console.WriteLine();

#if GenerateCode
            ParamClassGenerator.Generate();
#else

            var option = ConsolePrompt.Option("How do you want to input params", new string[] { "Use defaults", "Enter shareable JSON", "Enter manually" });
            ItemRandomizerParams randomizerParams = option switch
            {
                0 => new ItemRandomizerParams(),
                1 => GetShareableJSONParams(),
                2 => GetManuallyEnteredParams(),
                _ => null
            };

            if (randomizerParams == null)
            {
                return;
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

            Console.WriteLine();
            Console.Write("Press any key to close...");
            Console.ReadLine();
#endif
        }

        private static ItemRandomizerParams GetManuallyEnteredParams()
        {
            ItemRandomizerParams randomizerParams;
            int seed;

            if (ConsolePrompt.Bool("Generate random seed"))
            {
                seed = GenerateSeed();
            }
            else
            {
                seed = ConsolePrompt.Int("Seed");
            }

            randomizerParams = new ItemRandomizerParams()
            {
                Seed = seed,
                WeaponBaseDamageMultiplier = ConsolePrompt.Float("Weapon Base Damage Multiplier", 1),
                WeaponScalingMultiplier = ConsolePrompt.Float("Weapon Scaling Multiplier", 1)
            };

            return randomizerParams;
        }

        private static ItemRandomizerParams GetShareableJSONParams()
        {
            var randomizerParamsJson = ConsolePrompt.String("Enter randomizer params JSON string");
            var randomizerParams = JSON.Parse<ItemRandomizerParams>(randomizerParamsJson, out bool success);

            if (success)
            {
                var versionDiff = CompareVersions(randomizerParams.Version, Version);
                if (versionDiff != 0)
                {
                    var explain = versionDiff < 0 ? "less" : "greater";
                    if (!ConsolePrompt.Bool($"Params version is {explain} than current version ({randomizerParams.Version} vs {Version}). Continue"))
                    {
                        return null;
                    }
                }

                if (randomizerParams.Seed < 0)
                {
                    randomizerParams.Seed = GenerateSeed();
                    Console.WriteLine("Provided seed was invalid, generating new seed");
                }
            }
            else
            {
                randomizerParams = new ItemRandomizerParams();
                randomizerParams.Seed = GenerateSeed();
                Console.WriteLine("Provided params were invalid, using default randomizer params");
            }

            return randomizerParams;
        }

        private static void DrawProgressBar(float percent, string message)
        {
            Console.Write($"\r{message} ({percent * 100:F0}%)                     ");
        }

        private static int GenerateSeed()
        {
            return new Random().Next(1000000000);
        }

        // 0 if versions are identical. 1 if a is greater than b. -1 if a is less than b.
        public static int CompareVersions(string a, string b)
        {
            BreakVersion(a, out int aMajorVersion, out int aMinorVersion);
            BreakVersion(b, out int bMajorVersion, out int bMinorVersion);

            if (aMajorVersion == bMajorVersion)
            {
                return Math.Sign(aMinorVersion - bMinorVersion);
            }
            else
            {
                return Math.Sign(aMajorVersion - bMajorVersion);
            }
        }

        public static void BreakVersion(string version, out int majorVersion, out int minorVersion)
        {
            var split = version.Split('.');
            majorVersion = int.Parse(split[0]);
            minorVersion = int.Parse(split[1]);
        }
    }
}
