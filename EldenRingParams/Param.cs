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
        public Dictionary<int, string> RowNamesById;
        public List<PARAM.Row> Rows => Data.Rows;

        private PARAM Data;
        private ModContext ModContext;

        public ParamBase(ModContext modContext, BinderFile file)
        {
            ModContext = modContext;
            Data = PARAM.Read(file.Bytes);
            Name = Path.GetFileNameWithoutExtension(file.Name);
            string paramDefPath = Path.Combine(ModContext.ParamDefsDirectory, Name + ".xml");
            Data.ApplyParamdef(PARAMDEF.XmlDeserialize(paramDefPath));

            RowNamesById = new Dictionary<int, string>();
            string paramNamesPath = Path.Combine(ModContext.ParamNamesDirectory, Name + ".txt");
            var lines = File.ReadAllLines(paramNamesPath);
            foreach (var line in lines)
            {
                var split = line.Split(new char[] { ' ' }, 2);
                var id = int.Parse(split[0]);
                var name = split[1];
                RowNamesById.Add(id, name);
            }
        }

        public byte[] ToByteArray()
        {
            return Data.Write();
        }
    }
}
