using SoulsFormats;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EldenRingParams
{
    class ParamBase
    {
        public string Name;

        private Dictionary<int, RowBase> RowsById;
        private Dictionary<string, RowBase> RowsByName;
        private PARAM Data;
        private ModContext ModContext;

        public ParamBase(ModContext modContext, BinderFile file)
        {
            ModContext = modContext;
            Data = PARAM.Read(file.Bytes);
            Name = Path.GetFileNameWithoutExtension(file.Name);
            string paramDefPath = Path.Combine(ModContext.ParamDefsDirectory, Name + ".xml");
            Data.ApplyParamdef(PARAMDEF.XmlDeserialize(paramDefPath));

            var rowNamesById = new Dictionary<int, string>();
            string paramNamesPath = Path.Combine(ModContext.ParamNamesDirectory, Name + ".txt");
            var lines = File.ReadAllLines(paramNamesPath);
            foreach (var line in lines)
            {
                var split = line.Split(new char[] { ' ' }, 2);
                var id = int.Parse(split[0]);
                var name = split[1];
                rowNamesById.Add(id, name);
            }

            RowsById = new Dictionary<int, RowBase>();
            RowsByName = new Dictionary<string, RowBase>();
            foreach (var row in Data.Rows)
            {
                string name = null;
                if (rowNamesById.TryGetValue(row.ID, out string testName))
                {
                    name = testName;
                }
                
                var rowBase = new RowBase(row, name);
                RowsById.Add(rowBase.Id, rowBase);

                if (name != null)
                {
                    RowsByName.Add(name, rowBase);
                }
            }
        }

        public RowBase GetRowByName(string name)
        {
            if (RowsByName.TryGetValue(name, out RowBase rowBase))
            {
                return rowBase;
            }

            return null;
        }

        public RowBase GetRowById(int id)
        {
            if (RowsById.TryGetValue(id, out RowBase rowBase))
            {
                return rowBase;
            }

            return null;
        }

        public byte[] ToByteArray()
        {
            return Data.Write();
        }
    }
}
