using PropertyHook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EldenRingItemRandomizer
{
    public class SampleHook : PHook
    {
        public PHPointer EventFlagMan;

        public SampleHook() : base(5000, 1000, p => p.MainWindowTitle is "ELDEN RING™" or "ELDEN RING")
        {
            EventFlagMan = RegisterRelativeAOB(Offsets.EventFlagManAoB, Offsets.RelativePtrAddressOffset, Offsets.RelativePtrInstructionSize, 0x0);
        }
    }
}
