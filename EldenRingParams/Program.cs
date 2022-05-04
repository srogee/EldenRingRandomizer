using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SoulsFormats;

namespace EldenRingParams
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string gameDirectory = "Z:\\SteamLibrary\\steamapps\\common\\ELDEN RING\\Game";
            string paramDefsDirectory = "C:\\Users\\rid3r\\Downloads\\Yapped-Rune-Bear-2_1_0\\Paramdex\\ER";

            var modContext = new ModContext(gameDirectory, paramDefsDirectory);
            var regulation = new Regulation(modContext);

            regulation.Save();
        }
    }
}
