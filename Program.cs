//#define GenerateCode

using EldenRingItemRandomizer.GameState;
using SoulsFormats;
using StronglyTypedParams;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace EldenRingItemRandomizer
{
    partial class Program
    {
        public static string Version = "1.4";

        private static string Coalesce(params string[] values)
        {
            foreach (var value in values)
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    return value;
                }
            }

            return null;
        }

        static void Main(string[] args)
        {
#if GenerateCode
            ParamClassGenerator.Generate();
#else
            Console.WriteLine($"Elden Ring Item Randomizer v{Version}");
            Console.WriteLine();

            Preferences preferences = JSON.ParseFile<Preferences>("preferences.json");

            if (string.IsNullOrWhiteSpace(preferences.GameInstallDirectory) || !Directory.Exists(preferences.GameInstallDirectory))
            {
                Console.WriteLine("Game install directory is not set or does not exist. Set \"GameInstallDirectory\" in preferences.json.");
                Console.WriteLine();
                Console.Write("Press any key to close...");
                Console.ReadLine();
                return;
            }

            if (string.IsNullOrWhiteSpace(preferences.YappedDirectory) || !Directory.Exists(preferences.YappedDirectory))
            {
                Console.WriteLine("Yapped directory is not set or does not exist. Set \"YappedDirectory\" in preferences.json.");
                Console.WriteLine();
                Console.Write("Press any key to close...");
                Console.ReadLine();
                return;
            }

            ParamClassGenerator.YappedPath = preferences.YappedDirectory;

            string regulationInPath = Coalesce(preferences.BackupRegulationPath, Path.Combine(preferences.GameInstallDirectory, "regulation.bin.bak"));
            string regulationOutPath = Path.Combine(preferences.GameInstallDirectory, "regulation.bin");
            string exePath = Coalesce(preferences.EldenRingExePath, Path.Combine(preferences.GameInstallDirectory, "launch_elden_ring_seamlesscoop.exe"));

            if (!File.Exists(regulationInPath))
            {
                Console.WriteLine("Backup regulation file not found. Copy the vanilla regulation.bin and rename it to regulation.bin.bak, or set \"BackupRegulationPath\" in preferences.json.");
                Console.WriteLine();
                Console.Write("Press any key to close...");
                Console.ReadLine();
                return;
            }

            if (!File.Exists(exePath))
            {
                Console.WriteLine("Elden Ring .exe not found. Set \"EldenRingExePath\" in preferences.json.");
                Console.WriteLine();
                Console.Write("Press any key to close...");
                Console.ReadLine();
                return;
            }

            Console.WriteLine($"Found backup regulation file at \"{regulationInPath}\"");
            Console.WriteLine($"Found Elden Ring .exe at \"{exePath}\"");
            Console.WriteLine();

            var mainOption = ConsolePrompt.Option("What do you want to do", new string[] { "Randomize game files", "Run Elden Ring" });

            if (mainOption == 0)
            {
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
                    Console.WriteLine();
                    Console.Write("Press any key to close...");
                    Console.ReadLine();
                    return;
                }

                if (randomizerParams.Seed < 0)
                {
                    randomizerParams.Seed = GenerateSeed();
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

                Console.WriteLine(string.Join("\n", randomizer.SpoilerLog));
                Console.WriteLine();
                var runtime = new ItemRandomizerRuntime(regulationInPath, exePath);
                runtime.OnProgressChanged += DrawProgressBar;
                Console.WriteLine();
                runtime.Run();

                Console.WriteLine();
                Console.Write("Press any key to close...");
                Console.ReadLine();
            }
            else if (mainOption == 1)
            {
                var runtime = new ItemRandomizerRuntime(regulationInPath, exePath);
                runtime.OnProgressChanged += DrawProgressBar;
                Console.WriteLine();
                runtime.Run();

                Console.WriteLine();
                Console.Write("Press any key to close...");
                Console.ReadLine();
            }
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
                WeaponScalingMultiplier = ConsolePrompt.Float("Weapon Scaling Multiplier", 1),
                GreatRunesFromBossLegend = ConsolePrompt.Bool("Great Runes Drop From Demigods/Legends", true),
                GreatRunesFromBossGreatEnemy = ConsolePrompt.Bool("Great Runes Drop From (some) Great Enemies", true),
                GreatRunesFromBossField = ConsolePrompt.Bool("Great Runes Drop From (some) Field Bosses", true),
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
            }
            else
            {
                randomizerParams = new ItemRandomizerParams();
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
