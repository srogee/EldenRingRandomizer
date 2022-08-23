using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EldenRingItemRandomizer
{
    public class ItemAdditionalParams
    {
        public WepType[] WeaponTypes;
        public ProtectorCategory[] ArmorTypes;

        public ItemAdditionalParams(WepType[] weaponTypes, ProtectorCategory[] armorTypes)
        {
            WeaponTypes = weaponTypes;
            ArmorTypes = armorTypes;
        }
    }

    internal class ItemRandomizerHelper
    {
        private Random RandomNumberGenerator;
        private GameData GameData;
        private Dictionary<ItemType, HashSet<int>> ChosenItemsByType;
        private StronglyTypedParams.RegulationParams RegulationParams;
        private Dictionary<StronglyTypedParams.EquipParamWeapon, int> WeaponMaxUpgradeIdDict = new Dictionary<StronglyTypedParams.EquipParamWeapon, int>();

        public ItemRandomizerHelper(Random randomNumberGenerator, GameData gameData, StronglyTypedParams.RegulationParams regulationParams, Dictionary<StronglyTypedParams.EquipParamWeapon, int> weaponMaxUpgradeIdDict)
        {
            RandomNumberGenerator = randomNumberGenerator;
            GameData = gameData;
            ChosenItemsByType = new Dictionary<ItemType, HashSet<int>>();
            RegulationParams = regulationParams;
            WeaponMaxUpgradeIdDict = weaponMaxUpgradeIdDict;
        }

        // This is the API you want to use to get random items 99% of the time
        public ItemDescription GetRandomItemOfType(ItemType type, ItemAdditionalParams addlParams, bool withReplacement)
        {
            ItemDescription output = null;
            switch (type)
            {
                case ItemType.Weapon:
                    output = new ItemDescription(GetRandomItemOfTypeFromList(ItemType.Weapon, GetPotentialWeaponIdsForTypes(addlParams.WeaponTypes), withReplacement), ItemlotItemcategory.Weapon, ShopLineupEquiptype.Weapon);
                    break;
                case ItemType.Talisman:
                    output = new ItemDescription(GetRandomItemOfTypeFromList(ItemType.Talisman, GameData.Talismans, withReplacement), ItemlotItemcategory.Accessory, ShopLineupEquiptype.Accessory);
                    break;
                case ItemType.SpiritAsh:
                    output = new ItemDescription(GetRandomItemOfTypeFromList(ItemType.SpiritAsh, GameData.SpiritAshIds, withReplacement), ItemlotItemcategory.Good, ShopLineupEquiptype.Good);
                    break;
                case ItemType.Sorcery:
                    output = new ItemDescription(GetRandomItemOfTypeFromList(ItemType.Sorcery, GameData.SorceryIds, withReplacement), ItemlotItemcategory.Good, ShopLineupEquiptype.Good);
                    break;
                case ItemType.Incantation:
                    output = new ItemDescription(GetRandomItemOfTypeFromList(ItemType.Incantation, GameData.IncantationIds, withReplacement), ItemlotItemcategory.Good, ShopLineupEquiptype.Good);
                    break;
                case ItemType.PhysickTear:
                    output = new ItemDescription(GetRandomItemOfTypeFromList(ItemType.PhysickTear, GameData.Tears, withReplacement), ItemlotItemcategory.Good, ShopLineupEquiptype.Good);
                    break;
                case ItemType.AshOfWar:
                    output = new ItemDescription(GetRandomItemOfTypeFromList(ItemType.AshOfWar, GameData.AshOfWarIds, withReplacement), ItemlotItemcategory.Ash, ShopLineupEquiptype.Ashes);
                    break;
                case ItemType.Runes:
                    output = new ItemDescription(GetRandomItemOfTypeFromList(ItemType.Runes, GameData.GoodRuneIds, withReplacement), ItemlotItemcategory.Good, ShopLineupEquiptype.Good);
                    break;
                case ItemType.Armor:
                    output = new ItemDescription(GetRandomItemOfTypeFromList(ItemType.Armor, GetPotentialArmorIdsForTypes(addlParams.ArmorTypes), withReplacement), ItemlotItemcategory.Protector, ShopLineupEquiptype.Protector);
                    break;
            }

            if (output?.Id == -1)
            {
                return null;
            }

            return output;
        }

        public int GetRandomItemOfTypeFromList(ItemType type, IEnumerable<int> potentialItemIds, bool withReplacement)
        {
            int[] actualItemIds = null;
            // Even if you say with replacement, don't include items that have already been taken out
            if (!ChosenItemsByType.ContainsKey(type))
            {
                ChosenItemsByType.Add(type, new HashSet<int>());
            }

            var chosenItemHashset = ChosenItemsByType[type];
            actualItemIds = potentialItemIds.Where(id => !chosenItemHashset.Contains(id)).ToArray();

            if (actualItemIds.Length > 0)
            {
                var index = RandomNumberGenerator.Next(0, actualItemIds.Length);
                var itemId = actualItemIds[index];

                if (!withReplacement)
                {
                    ChosenItemsByType[type].Add(itemId);
                }

                return itemId;
            }

            return -1;
        }

        // This is the API you want to use to get random items 99% of the time
        public ItemDescription GetRandomItemWeighted(ItemTypeAndWeight[] itemWeights, ItemAdditionalParams addlParams, bool withReplacement)
        {
            var itemType = GetRandomItemTypeWeighted(itemWeights, addlParams);
            if (itemType != null)
            {
                return GetRandomItemOfType(itemType.Value, addlParams, withReplacement || itemWeights.First(weight => weight.Type == itemType.Value).WithReplacement);
            }

            return null;
        }

        private ItemType? GetRandomItemTypeWeighted(IEnumerable<ItemTypeAndWeight> itemWeights, ItemAdditionalParams addlParams)
        {
            // Get array of item weights filtered by which item types still have items to be chosen
            var filtered = itemWeights.Where(item => GetRandomItemOfType(item.Type, addlParams, true) != null).ToArray();
            var index = RandomUtils.ChooseRandomWeighted(RandomNumberGenerator, filtered.Select(item => item.Weight).ToArray());
            if (index >= 0 && index < filtered.Length)
            {
                return filtered[index].Type;
            }
            else
            {
                return null;
            }
        }

        // Return list of valid, fully upgraded weapons that match the specified types
        private IEnumerable<int> GetPotentialWeaponIdsForTypes(WepType[] wepTypes)
        {
            var typesHashset = new HashSet<WepType>(wepTypes);
            return RegulationParams.EquipParamWeapon.Where(weapon => weapon.RowName?.Length > 0 && typesHashset.Contains(weapon.WeaponType)).Select(weapon => WeaponMaxUpgradeIdDict[weapon]);
        }

        // Return list of valid armor that matches the specified types
        private IEnumerable<int> GetPotentialArmorIdsForTypes(ProtectorCategory[] protectorTypes)
        {
            var typesHashset = new HashSet<ProtectorCategory>(protectorTypes);
            return RegulationParams.EquipParamProtector.Where(weapon => weapon.RowName?.Length > 0 && typesHashset.Contains(weapon.ArmorCategory)).Select(weapon => weapon.Id);
        }
    }

    // Helper enum to distinguish between types of items
    public enum ItemType
    {
        SpiritAsh,
        Talisman,
        PhysickTear,
        Sorcery,
        Incantation,
        Weapon,
        AshOfWar,
        Runes,
        Armor
    }

    // Helper class to describe an item
    public class ItemDescription
    {
        public int Id;
        public ItemlotItemcategory Category;
        public ShopLineupEquiptype EquipType;

        public ItemDescription(int id, ItemlotItemcategory category, ShopLineupEquiptype equipType)
        {
            Id = id;
            Category = category;
            EquipType = equipType;
        }
    }

    // Helper class to describe a type of item and its chance
    public class ItemTypeAndWeight
    {
        public ItemType Type;
        public float Weight;
        public bool WithReplacement;

        public ItemTypeAndWeight(ItemType type, float weight, bool withReplacement = false)
        {
            Type = type;
            Weight = weight;
            WithReplacement = withReplacement;
        }
    }
}
