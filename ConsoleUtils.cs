using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EldenRingItemRandomizer
{
    public static class ConsoleUtils
    {
        private static int UpdateLineStart = -1;

        public static void WriteLine(string text)
        {
            if (UpdateLineStart != -1 && text.Length < Console.WindowWidth)
            {
                for (int i = text.Length; i < Console.WindowWidth; i++)
                {
                    text += " ";
                }
            }

            Console.WriteLine(text);
        }

        public static void StartOverwrite()
        {
            if (UpdateLineStart == -1)
            {
                UpdateLineStart = Console.CursorTop;
            }
            else
            {
                Console.SetCursorPosition(0, UpdateLineStart);
            }
        }

        public static void StopOverwrite()
        {
            UpdateLineStart = -1;
        }
    }
}
