//#define GenerateCode

#if GenerateCode
#else

using StronglyTypedParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EldenRingItemRandomizer
{
    class ItemRandomizer
    {
        private Dictionary<int, EquipParamWeapon> ReverseWeaponIndex = new Dictionary<int, EquipParamWeapon>();
        private Dictionary<EquipParamWeapon, int> WeaponMaxUpgradeIdDict = new Dictionary<EquipParamWeapon, int>();
        private List<int> MasterWeaponList = new List<int>();
        private RegulationParams RegulationParams;

        public class ChurchItemLots
        {
            public List<int> ItemLotIds;
            public ChurchItemLots(params int[] itemLotIds)
            {
                ItemLotIds = new List<int>(itemLotIds);
            }
        }

        private List<ChurchItemLots> Churches = new List<ChurchItemLots>(new ChurchItemLots[] {
            new ChurchItemLots(1037460000, 1037460001), // Church of vows TODO - what about 1037460010 stormhawk feather?
            new ChurchItemLots(1037490030, 1037490031, 1037490032, 1037490100), // church of inhibition
            new ChurchItemLots(1035440200) // rose church
        });

        public void Run()
        {
            Console.WriteLine($"Loading regulation file...");
            RegulationParams = RegulationParams.Load(ParamClassGenerator.RegulationInPath);

            // EquipParamGoods
            // 8600 - 8618 maps

            // Gather info about max upgrade levels
            Console.WriteLine($"Modifying weapons...");

            foreach (var weapon in RegulationParams.EquipParamWeapon)
            {
                ProcessWeapon(weapon);
            }

            Console.WriteLine($"Modifying item lots...");
            foreach (var itemLot in RegulationParams.ItemLotParam_enemy)
            {
                ProcessItemLot(itemLot);
            }

            foreach (var itemLot in RegulationParams.ItemLotParam_map)
            {
                ProcessItemLot(itemLot);
            }

            // Change auto picked up flask amounts
            RegulationParams.ItemLotParam_map[2000].ItemAmount1 = 12;
            RegulationParams.ItemLotParam_map[2001].ItemAmount1 = 2;

            ModifyStartingClasses();

            Console.WriteLine($"Saving regulation file...");
            RegulationParams.Save(ParamClassGenerator.RegulationOutPath);

            Console.WriteLine($"Done");
        }

        private void ModifyStartingClasses()
        {
            Console.WriteLine($"Modifying starting classes...");
            for (int classId = 3000; classId <= 3009; classId++)
            {
                var startingClass = RegulationParams.CharaInitParam[classId];

                // Wretch stats
                startingClass.Level = 1;
                startingClass.Vigor = 10;
                startingClass.Attunement = 10;
                startingClass.Endurance = 10;
                startingClass.Strength = 10;
                startingClass.Dexterity = 10;
                startingClass.Intelligence = 10;
                startingClass.Faith = 10;
                startingClass.Arcane = 10;

                // Weapons
                var uchigatana = RegulationParams.EquipParamWeapon[9000000];
                startingClass.EquippedWeaponRightPrimary = WeaponMaxUpgradeIdDict[uchigatana];
                startingClass.EquippedWeaponRightSecondary = 9000000;
                startingClass.EquippedWeaponRightTertiary = 9000000;
                startingClass.EquippedWeaponLeftPrimary = 9000000;
                startingClass.EquippedWeaponLeftSecondary = 9000000;
                startingClass.EquippedWeaponLeftTertiary = 9000000;

                // Flash of wondrous physick
                startingClass.EquippedItemSlot1 = 251;
                startingClass.EquippedItemSlot1Amount = 1;

                // Spirit calling bell
                startingClass.StoredItemSlot2 = 8158;
                startingClass.StoredItemSlot2Count = 1;

                // Talisman pouches
                startingClass.StoredItemSlot3 = 10040;
                startingClass.StoredItemSlot3Count = 3;

                // Spectral steed whistle
                startingClass.StoredItemSlot4 = 130;
                startingClass.StoredItemSlot4Count = 1;

                startingClass.HPFlaskMaxPossessionLimit = 12;
                startingClass.FPFlaskMaxPossessionLimit = 2;
            }
        }

        // Gather info about max upgrade level for the weapon, and remove its starting requirements
        void ProcessWeapon(EquipParamWeapon weapon)
        {
            if (weapon.WeaponType == WepType.None)
            {
                return;
            }

            MasterWeaponList.Add(weapon.Id);
            
            weapon.RequirementSTR = 10;
            weapon.RequirementDEX = 10;
            weapon.RequirementFTH = 10;
            weapon.RequirementINT = 10;
            weapon.RequirementARC = 10;

            int maxUpgradeId = weapon.Id;

            for (int i = 0; i <= 25; i++)
            {
                var originUpgradeCell = weapon[$"OriginWeapon{i}"];
                if (originUpgradeCell != null)
                {
                    var originWeapon = RegulationParams.EquipParamWeapon[(int)originUpgradeCell.Value];
                    if (originWeapon != null)
                    {
                        maxUpgradeId = weapon.Id + i;
                        ReverseWeaponIndex.Add(maxUpgradeId, weapon);
                    }
                }
            }

            WeaponMaxUpgradeIdDict.Add(weapon, maxUpgradeId);
        }

        // Set all weapons in the item lot to be their max upgraded equivalents
        void ProcessItemLot(ItemLotParam itemLot)
        {
            for (int i = 1; i <= 8; i++)
            {
                itemLot[$"ItemID{i}"].Value = 0;
                itemLot[$"ItemChance{i}"].Value = 0;
                itemLot[$"ItemAmount{i}"].Value = 0;
                itemLot[$"ItemCategory{i}"].Value = ItemlotItemcategory.None;

                //var itemCell = itemLot[$"ItemID{i}"];
                //if (itemCell != null)
                //{
                //    var id = (int)itemCell.Value;
                //    if (ReverseWeaponIndex.TryGetValue(id, out EquipParamWeapon weapon))
                //    {
                //        itemCell.Value = WeaponMaxUpgradeIdDict[weapon];
                //    }
                //}
            }
        }
    }
}
#endif