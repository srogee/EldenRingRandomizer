using SoulsFormats;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EldenRingParams
{
    class Regulation
    {
        private BND4 Data;
        private ModContext ModContext;
        private Dictionary<string, ParamBase> ParamsByName;

        public ParamBase EquipParamWeapon => LoadParamByName("EquipParamWeapon"); // Weapon definitions
        public ParamBase CharaInitParam => LoadParamByName("CharaInitParam"); // Starting classes + NPC inventories
        public ParamBase ItemLotParam_enemy => LoadParamByName("ItemLotParam_enemy"); // Random items that can be dropped by enemies
        public ParamBase ItemLotParam_map => LoadParamByName("ItemLotParam_map"); // Items dropped by specific enemies and placed in specific places

        public Regulation(ModContext modContext)
        {
            ModContext = modContext;
            ParamsByName = new Dictionary<string, ParamBase>();
        }

        private ParamBase LoadParamByName(string name)
        {
            if (ParamsByName.ContainsKey(name))
            {
                return ParamsByName[name];
            }

            LoadData();

            BinderFile paramFile = Data.Files.Find(file => Path.GetFileNameWithoutExtension(file.Name) == name);
            if (paramFile == null)
            {
                throw new Exception($"Could not find param file \"{name}\" in binder");
            }

            ParamBase param = new ParamBase(ModContext, paramFile);
            ParamsByName.Add(param.Name, param);

            return param;
        }

        private void LoadData()
        {
            if (Data == null)
            {
                Console.WriteLine($"Decrypting {ModContext.RegulationInPath}...");
                Data = SFUtil.DecryptERRegulation(ModContext.RegulationInPath);
            }
        }

        public void Save()
        {
            // Save param changes into binder files
            Console.WriteLine($"Saving params...");
            foreach (var entry in ParamsByName)
            {
                var binderFile = Data.Files.FirstOrDefault(file => Path.GetFileNameWithoutExtension(file.Name) == entry.Key);
                if (binderFile != null)
                {
                    binderFile.Bytes = entry.Value.ToByteArray();
                }
            }

            Console.WriteLine($"Encrypting {ModContext.RegulationOutPath}...");
            SFUtil.EncryptERRegulation(ModContext.RegulationOutPath, Data);
        }

        public PARAM.Row GetRowById(string paramName, int id)
        {
            return ParamsByName[paramName].Rows.FirstOrDefault(row => row.ID == id);
        }

        public string GetRowName(string paramName, int id)
        {
            return "???";
        }

        public PARAM.Cell GetCellByDisplayName(PARAM.Row row, string displayName)
        {
            return row.Cells.FirstOrDefault(cell => cell.Def.DisplayName == displayName);
        }
    }
}
