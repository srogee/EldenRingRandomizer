using SoulsFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EldenRingParams
{
    class RowBase
    {
        private PARAM.Row Data;
        public int Id => Data.ID;
        public string Name;

        private Dictionary<string, CellBase> CellsById;
        private Dictionary<string, CellBase> CellsByName;

        public RowBase(PARAM.Row data, string name)
        {
            Data = data;
            Name = name;

            CellsById = new Dictionary<string, CellBase>();
            CellsByName = new Dictionary<string, CellBase>();
            foreach (var cell in data.Cells)
            {
                var cellBase = new CellBase(cell);
                CellsById.Add(cellBase.Id, cellBase);
                CellsByName.Add(cellBase.Name, cellBase);
            }
        }

        public CellBase GetCellByName(string name)
        {
            if (CellsByName.TryGetValue(name, out CellBase cellBase))
            {
                return cellBase;
            }

            return null;
        }

        public CellBase GetCellById(string id)
        {
            if (CellsById.TryGetValue(id, out CellBase cellBase))
            {
                return cellBase;
            }

            return null;
        }
    }
}
