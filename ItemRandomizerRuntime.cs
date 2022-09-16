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
                    break;
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
        }

        private void OnHookedAndLoaded()
        {
            WarpToRoundtableHold();
            UnlockAllMapFragments();
            UnlockAllMapPoints();
            UnlockAllWhetblades();
            UnlockTorrent();
            GrantUpgradedFlasks();
            OnProgressChanged(1, "Done");
        }

        private void WarpToRoundtableHold()
        {
            CurrentTaskIndex++;
            UpdateProgress(0);

            // set the first step, church of elleh, stranded graveyard and gatefront as unlocked
            Hook.SetEventFlag(76100, true);
            Hook.SetEventFlag(76101, true);
            Hook.SetEventFlag(76111, true);
            Hook.SetEventFlag(71801, true);

            // warp to roundtable
            Hook.Warp(11101950);
        }

        private void UnlockAllMapFragments()
        {
            CurrentTaskIndex++;
            UpdateProgress(0);

            foreach (var flag in GameData.MapFragmentFlags)
            {
                Hook.SetEventFlag(flag, true);
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
    }
}
