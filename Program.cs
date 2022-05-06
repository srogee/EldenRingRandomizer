using SoulsFormats;
using StronglyTypedParams;
using System;
using System.Linq;

namespace EldenRingItemRandomizer
{
    partial class Program
    {
        private enum Mode
        {
            GenerateCode = 1,
            RunMod = 0
        }

        private const string RegulationInPath = @"Z:\SteamLibrary\steamapps\common\ELDEN RING\Game\regulation.bin.bak";
        private const string RegulationOutPath = @"Z:\SteamLibrary\steamapps\common\ELDEN RING\Game\regulation.bin";

        static void Main(string[] args)
        {
            var mode = (Mode)0;
            if (mode == Mode.RunMod)
            {
                Console.WriteLine($"Decryping regulation file...");
                var binder = SFUtil.DecryptERRegulation(RegulationInPath);
                var regulationParams = new RegulationParams(binder);

                var uchigatana = regulationParams.EquipParamWeapon[9000000];
                Console.WriteLine(uchigatana.DamagePhysical);
            }
            else
            {
                ParamClassGenerator.Generate();
            }
        }
    }
}
