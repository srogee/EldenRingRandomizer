using SoulsFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EldenRingParams
{
    class CellBase
    {
        public string Id => Data.Def.InternalName;
        public string Name => Data.Def.DisplayName;
        public object Value => Data.Value;

        private PARAM.Cell Data;

        public CellBase(PARAM.Cell data)
        {
            Data = data;
        }
    }
}
