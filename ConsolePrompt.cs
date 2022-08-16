using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EldenRingItemRandomizer
{
    internal class ConsolePrompt
    {
        public static string String(string prompt, string defaultValue = "")
        {
            var value = PromptValue(prompt);
            if (value.Trim().Length > 0)
            {
                return value;
            }
            else
            {
                return defaultValue;
            }
        }

        public static bool Bool(string prompt, bool defaultValue = false)
        {
            var value = PromptValue($"{prompt} (y/n)? ", false).ToLower();
            if (value == "y" || value == "yes")
            {
                return true;
            }
            else if (value == "n" || value == "no")
            {
                return false;
            }
            else
            {
                return defaultValue;
            }
        }

        public static float Float(string prompt, float defaultValue = 0.0f)
        {
            var value = PromptValue(prompt);
            if (float.TryParse(value, out float result))
            {
                return result;
            }
            else
            {
                return defaultValue;
            }
        }

        public static int Int(string prompt, int defaultValue = 0)
        {
            var value = PromptValue(prompt);
            if (int.TryParse(value, out int result))
            {
                return result;
            }
            else
            {
                return defaultValue;
            }
        }

        private static string PromptValue(string prompt, bool includeColonAndSpace = true)
        {
            if (includeColonAndSpace)
            {
                Console.Write($"{prompt}: ");
            }
            else
            {
                Console.Write(prompt);
            }
            string value = Console.ReadLine();
            return value;
        }
    }
}
