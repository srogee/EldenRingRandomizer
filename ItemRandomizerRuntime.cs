using EldenRingItemRandomizer.GameState;
using StronglyTypedParams;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace EldenRingItemRandomizer
{
    internal class ItemRandomizerRuntime
    {
        private int CurrentTaskIndex = 0;
        private EldenRingHook Hook;
        private bool HookedAndLoadedRaised = false;
        private string RegulationPath;
        private RegulationParams RegulationParams;
        private GameData GameData;
        public delegate void ProgressChangedEvent(float precent, string message);
        public ProgressChangedEvent OnProgressChanged;
        private string ExePath;
        private bool HookedRaised = false;
        private RandomizerGameState RandomizedGameState;

        private static TaskDefinition[] Tasks = new TaskDefinition[] {
            new TaskDefinition("Waiting for Elden Ring to start"),
            new TaskDefinition("Loading regulation file"),
            new TaskDefinition("Waiting for player memory location"),
            new TaskDefinition("Warping to Roundtable Hold"),
            new TaskDefinition("Unlocking Map Fragments"),
            new TaskDefinition("Unlocking Map Locations"),
            new TaskDefinition("Unlocking Whetblades"),
            new TaskDefinition("Unlocking Torrent"),
            new TaskDefinition("Unlocking world"),
            new TaskDefinition("Granting Flasks"),
        };

        public ItemRandomizerRuntime(string regulationPath, string exePath)
        {
            ExePath = exePath;
            Hook = new EldenRingHook(1000, 1000, p => p.MainWindowTitle is "ELDEN RING™" or "ELDEN RING");
            RegulationPath = regulationPath;
            RandomizedGameState = JSON.ParseFile<RandomizerGameState>("state.json");
        }

        public void Run()
        {
            CurrentTaskIndex = -1;
            StartEldenRing(ExePath);
            Hook.Start();

            while (true)
            {
                if (!HookedRaised && Hook.Hooked)
                {
                    // Do first time setup when hooked for the first time
                    OnHooked();
                    HookedRaised = true;
                }

                if (!HookedAndLoadedRaised && Hook.Hooked && Hook.Loaded)
                {
                    // Do first time setup when hooked and player is valid
                    OnHookedAndLoaded();
                    HookedAndLoadedRaised = true;
                }

                if (HookedAndLoadedRaised)
                {
                    Console.WriteLine();
                    Console.WriteLine();
                    ConsoleUtils.StartOverwrite();

                    Console.WriteLine("Randomizer Progress");
                    foreach (var pair in RandomizedGameState.BossDefinitionGreatRunePairs)
                    {
                        var boss = GameData.RandomizedBosses[pair.Item1];
                        var greatRune = GameData.GreatRunes[pair.Item2];
                        var acquired = Hook.GetEventFlag(greatRune.EventId);
                        ConsoleUtils.WriteLine($"{boss.Name} ({greatRune.Name}) - {(acquired ? "Defeated" : "Not Defeated")}");
                    }

                    var unlocked = Hook.GetEventFlag(GameData.FinalBossSiteOfGrace.EventId);
                    ConsoleUtils.WriteLine("");
                    ConsoleUtils.WriteLine($"Final Boss - {(unlocked ? "Unlocked" : "Not Unlocked")}");

                    // Unlock Ashen Capital when all great runes are acquired
                    if (ShouldUnlockEndgame())
                    {
                        Hook.SetEventFlag(GameData.FinalBossSiteOfGrace.EventId, true);
                    }
                }

                Thread.Sleep(1000); // Don't thrash the CPU
            }
        }

        private void UpdateProgress(float taskProgress)
        {
            OnProgressChanged((CurrentTaskIndex + taskProgress) / Tasks.Length, Tasks[CurrentTaskIndex].Name);
        }

        private void StartEldenRing(string exePath)
        {
            CurrentTaskIndex++;
            UpdateProgress(0);

            var info = new ProcessStartInfo();
            info.WorkingDirectory = Path.GetDirectoryName(exePath);
            info.UseShellExecute = true;
            info.WindowStyle = ProcessWindowStyle.Hidden;
            info.FileName = Path.GetFileName(exePath);
            Process.Start(info);
        }

        private void LoadRegulationFile()
        {
            CurrentTaskIndex++;
            UpdateProgress(0);

            RegulationParams = RegulationParams.Load(RegulationPath);
            GameData = new GameData(RegulationParams);
        }

        private void OnHooked()
        {
            LoadRegulationFile();

            CurrentTaskIndex++;
            UpdateProgress(0);
            // Wait for player memory location
        }

        private void OnHookedAndLoaded()
        {
            WarpToRoundtableHold();
            UnlockAllMapFragments();
            UnlockAllMapPoints();
            UnlockAllWhetblades();
            UnlockTorrent();
            UnlockWorld();
            GrantUpgradedFlasks();
            OnProgressChanged(1, "Done");
        }

        private void WarpToRoundtableHold()
        {
            CurrentTaskIndex++;
            UpdateProgress(0);

            // warp to roundtable
            Hook.Warp(11101950);
        }

        private void UnlockAllMapFragments()
        {
            CurrentTaskIndex++;
            UpdateProgress(0);

            foreach (var item in GameData.MapFragments)
            {
                Hook.GiveItem(item.GetItemSpawnInfo());
            }
        }

        private void UnlockAllMapPoints()
        {
            CurrentTaskIndex++;
            UpdateProgress(0);

            foreach (var flag in GameData.MapPointFlags)
            {
                Hook.SetEventFlag(flag, true);
            }
        }

        private void UnlockAllWhetblades()
        {
            CurrentTaskIndex++;
            UpdateProgress(0);

            Hook.GiveItem(new ItemSpawnInfo(8590, Category.Goods, 1, 1, 0, 0, -1, 400210)); // Whetstone Knife
            Hook.GiveItem(new ItemSpawnInfo(8970, Category.Goods, 1, 1, 0, 0, -1, 65610)); // Iron Whetblade
            Hook.GiveItem(new ItemSpawnInfo(8972, Category.Goods, 1, 1, 0, 0, -1, 65660)); // Sanctified Whetblade
            Hook.GiveItem(new ItemSpawnInfo(8974, Category.Goods, 1, 1, 0, 0, -1, 65720)); // Black Whetblade
            Hook.GiveItem(new ItemSpawnInfo(8973, Category.Goods, 1, 1, 0, 0, -1, 65680)); // Glintstone Whetblade
            Hook.GiveItem(new ItemSpawnInfo(8971, Category.Goods, 1, 1, 0, 0, -1, 65640)); // Red-Hot Whetblade
        }

        private void UnlockTorrent()
        {
            CurrentTaskIndex++;
            UpdateProgress(0);

            Hook.GiveItem(new ItemSpawnInfo(130, Category.Goods, 1, 1, 0, 0, -1, 60100));
        }

        private void GrantUpgradedFlasks()
        {
            CurrentTaskIndex++;
            UpdateProgress(0);

            Hook.GiveItem(new ItemSpawnInfo(1025, Category.Goods, 12, 14, 0, 0, -1, 60000));
            Hook.GiveItem(new ItemSpawnInfo(1075, Category.Goods, 2, 14, 0, 0, -1, 60000));
        }

        private void UnlockWorld()
        {
            CurrentTaskIndex++;
            UpdateProgress(0);

            foreach (var keyItem in GameData.KeyItems)
            {
                Hook.GiveItem(keyItem.GetItemSpawnInfo());
            }

            foreach (var siteOfGrace in GameData.UnlockedSitesOfGrace)
            {
                Hook.SetEventFlag(siteOfGrace.EventId, true);
            }

            // Unlock door to Lyendell
            Hook.SetEventFlag(105, true);
            Hook.SetEventFlag(182, true);
        }

        private bool ShouldUnlockEndgame()
        {
            var greatRunes = RandomizedGameState.BossDefinitionGreatRunePairs.Select(pair => GameData.GreatRunes[pair.Item2]);

            return greatRunes.All(greatRune => Hook.GetEventFlag(greatRune.EventId));
        }
    }
}
