using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SoulsFormats;
using System.IO;

namespace EldenRingParams
{
    internal class ModContext
    {
        public string RegulationInPath;
        public string RegulationOutPath;
        public string ParamDefsDirectory;
        public string ParamNamesDirectory;

        public ModContext(string gameDirectory, string paramDexDirectory)
        {
            RegulationInPath = Path.Combine(gameDirectory, "regulation.bin.bak"); // Uses backup file as base so we start from scratch every time
            RegulationOutPath = Path.Combine(gameDirectory, "regulation.bin");
            ParamDefsDirectory = Path.Combine(paramDexDirectory, "Defs");
            ParamNamesDirectory = Path.Combine(paramDexDirectory, "Names");
        }
    }
}
