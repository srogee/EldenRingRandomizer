using EldenRingItemRandomizer.GameState;
using StronglyTypedParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EldenRingItemRandomizer
{
    internal class ItemRandomizerRuntime
    {
        private EldenRingHook Hook;
        private bool HookedAndLoadedRaised = false;
        private string RegulationPath;
        private RegulationParams RegulationParams;
        private GameData GameData;

        public ItemRandomizerRuntime(string regulationPath)
        {
            Hook = new EldenRingHook(1000, 1000, p => p.MainWindowTitle is "ELDEN RING™" or "ELDEN RING");
            RegulationPath = regulationPath;
        }

        public void Run()
        {
            LoadRegulationFile();
            Hook.Start();

            Console.WriteLine("Waiting for player memory pointer to become valid...");
            while (true)
            {
                if (!HookedAndLoadedRaised && Hook.Hooked && Hook.Loaded)
                {
                    // Do first time setup when hooked and player is valid
                    OnHookedAndLoaded();
                    HookedAndLoadedRaised = true;
                    break;
                }

                if (!Hook.Hooked)
                {
                    // Reset flag if we're no longer hooked
                    HookedAndLoadedRaised = false;
                }

                Thread.Sleep(1000); // Don't thrash the CPU
            }
        }

        private void OnHookedAndLoaded()
        {
            WarpToRoundtableHold();
            UnlockAllMapFragments();
            UnlockAllMapPoints();
            UnlockAllWhetblades();
        }

        private void LoadRegulationFile()
        {
            Console.WriteLine("Loading regulation file...");
            RegulationParams = RegulationParams.Load(RegulationPath);
            GameData = new GameData(RegulationParams);
        }

        private void UnlockAllMapFragments()
        {
            Console.WriteLine("Unlocking map fragments...");
            foreach (var flag in GameData.MapFragmentFlags)
            {
                Hook.SetEventFlag(flag, true);
            }
        }

        private void UnlockAllMapPoints()
        {
            Console.WriteLine("Unlocking map points...");
            foreach (var flag in GameData.MapPointFlags)
            {
                Hook.SetEventFlag(flag, true);
            }
        }

        // TODO this doesn't work...
        private void UnlockAllWhetblades()
        {
            //Console.WriteLine("Unlocking whetblades...");
            //foreach (var flag in GameData.WhetbladeFlagIds)
            //{
            //    Hook.SetEventFlag(flag, true);
            //}
        }

        private void WarpToRoundtableHold()
        {
            // set the first step, church of elleh, and gatefront as unlocked
            Hook.SetEventFlag(76100, true);
            Hook.SetEventFlag(76101, true);
            Hook.SetEventFlag(76111, true);

            // warp to roundtable
            Hook.Warp(11101950);
        }
    }
}
