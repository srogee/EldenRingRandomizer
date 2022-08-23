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
    }
}
