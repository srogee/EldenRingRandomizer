using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EldenRingItemRandomizer
{
    internal class RandomUtils
    {
        public static int ChooseRandomWeighted(Random randomNumberGenerator, float[] weights)
        {
            float totalWeight = weights.Sum();
            float random = (float)randomNumberGenerator.NextDouble() * totalWeight;
            float prevWeight = 0;
            float nextWeight;
            for (int i = 0; i < weights.Length; i++)
            {
                nextWeight = prevWeight + weights[i];

                if (random >= prevWeight && random <= nextWeight)
                {
                    return i;
                }

                prevWeight = nextWeight;
            }

            return -1;
        }

        public static void Shuffle<T>(Random randomNumberGenerator, T[] array)
        {
            int n = array.Length;
            for (int i = 0; i < (n - 1); i++)
            {
                // Use Next on random instance with an argument.
                // ... The argument is an exclusive bound.
                //     So we will not go past the end of the array.
                int r = i + randomNumberGenerator.Next(n - i);
                T t = array[r];
                array[r] = array[i];
                array[i] = t;
            }
        }
    }
}
