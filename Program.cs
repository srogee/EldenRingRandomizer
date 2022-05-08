//#define GenerateCode

using SoulsFormats;
using StronglyTypedParams;
using System;
using System.Collections.Generic;
using System.IO;

namespace EldenRingItemRandomizer
{
    partial class Program
    {


        static void Main(string[] args)
        {
#if GenerateCode
            ParamClassGenerator.Generate();
#else
            var randomizer = new ItemRandomizer();
            randomizer.Run();
#endif
        }
    }
}
