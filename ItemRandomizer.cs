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
        private RegulationParams RegulationParams;
        private Dictionary<ItemType, HashSet<int>> ChosenItems;

        private WepType[] GoodWeaponTypes = new WepType[] {
            WepType.StraightSword,
            WepType.Greatsword,
            WepType.ColossalSword,
            WepType.ColossalWeapon,
            WepType.CurvedSword,
            WepType.CurvedGreatsword,
            WepType.Katana,
            WepType.Twinblade,
            WepType.ThrustingSword,
            WepType.HeavyThrustingSword,
            WepType.Axe,
            WepType.Greataxe,
            WepType.Hammer,
            WepType.GreatHammer,
            WepType.Flail,
            WepType.Spear,
            WepType.HeavySpear,
            WepType.Halberd,
            WepType.Scythe,
            WepType.ColossalWeapon,
            WepType.Staff,
            WepType.Seal
        };

        private Random RandomNumberGenerator;

        private GameData GameData;

        private ItemTypeAndWeight[] ChurchWeights = new ItemTypeAndWeight[]
        {
            new ItemTypeAndWeight(ItemType.Weapon, 2f),
            new ItemTypeAndWeight(ItemType.SpiritAsh, 1.0f),
            new ItemTypeAndWeight(ItemType.AshOfWar, 1.0f),
            new ItemTypeAndWeight(ItemType.Runes, 0.5f, true),
        };

        private ItemTypeAndWeight[] ScarabWeights = new ItemTypeAndWeight[]
        {
            new ItemTypeAndWeight(ItemType.PhysickTear, 1.0f),
            new ItemTypeAndWeight(ItemType.Talisman, 1.0f),
            new ItemTypeAndWeight(ItemType.AshOfWar, 1.0f),
            new ItemTypeAndWeight(ItemType.Runes, 0.5f, true),
        };

        private ItemTypeAndWeight[] MinorBossWeights = new ItemTypeAndWeight[]
        {
            new ItemTypeAndWeight(ItemType.Weapon, 2f),
            new ItemTypeAndWeight(ItemType.SpiritAsh, 1.0f),
            new ItemTypeAndWeight(ItemType.AshOfWar, 1.0f),
            new ItemTypeAndWeight(ItemType.Talisman, 1.0f),
            new ItemTypeAndWeight(ItemType.Runes, 0.5f, true),
        };

        private ItemTypeAndWeight[] CarriageWeights = new ItemTypeAndWeight[]
        {
            new ItemTypeAndWeight(ItemType.Weapon, 2f),
            new ItemTypeAndWeight(ItemType.SpiritAsh, 1.0f),
            new ItemTypeAndWeight(ItemType.Runes, 0.5f, true),
        };

        private ItemTypeAndWeight[] MajorBossWeights = new ItemTypeAndWeight[]
        {
            new ItemTypeAndWeight(ItemType.Weapon, 2f),
            new ItemTypeAndWeight(ItemType.SpiritAsh, 1.0f),
            new ItemTypeAndWeight(ItemType.Runes, 0.5f, true),
        };

        private ItemTypeAndWeight[] GoldenTreeWeights = new ItemTypeAndWeight[]
        {
            new ItemTypeAndWeight(ItemType.PhysickTear, 1.0f),
            new ItemTypeAndWeight(ItemType.AshOfWar, 1.0f),
            new ItemTypeAndWeight(ItemType.Runes, 0.5f, true),
        };

        private ItemTypeAndWeight[] MapPlinthWeights = new ItemTypeAndWeight[]
        {
            new ItemTypeAndWeight(ItemType.Weapon, 2f),
            new ItemTypeAndWeight(ItemType.Runes, 0.5f, true),
        };

        private ItemTypeAndWeight[] ShopWeights = new ItemTypeAndWeight[]
        {
            new ItemTypeAndWeight(ItemType.Weapon, 0.5f),
            new ItemTypeAndWeight(ItemType.Sorcery, 1.0f),
            new ItemTypeAndWeight(ItemType.Incantation, 1.0f),
            new ItemTypeAndWeight(ItemType.Talisman, 1.0f),
            new ItemTypeAndWeight(ItemType.PhysickTear, 1.0f),
            new ItemTypeAndWeight(ItemType.AshOfWar, 1.0f),
            new ItemTypeAndWeight(ItemType.SpiritAsh, 1.0f),
        };

        private ItemTypeAndWeight[] AllWeights = new ItemTypeAndWeight[]
        {
            new ItemTypeAndWeight(ItemType.Weapon, 1.0f),
            new ItemTypeAndWeight(ItemType.AshOfWar, 0.25f),
            new ItemTypeAndWeight(ItemType.Runes, 0.25f),
        };

        public void Run()
        {
            ChosenItems = new Dictionary<ItemType, HashSet<int>>();

            Console.WriteLine($"Loading regulation file...");
            RegulationParams = RegulationParams.Load(ParamClassGenerator.RegulationInPath);

            GameData = new GameData(RegulationParams);

            int Seed = new Random().Next();
            Console.WriteLine($"Using seed {Seed}");
            RandomNumberGenerator = new Random(Seed);

            // EquipParamGoods
            // 8600 - 8618 maps

            // Gather info about max upgrade levels and remove stat requirements
            Console.WriteLine($"Modifying weapons...");
            foreach (var weapon in RegulationParams.EquipParamWeapon)
            {
                ProcessWeapon(weapon);
            }

            // Remove stat requirements
            Console.WriteLine($"Modifying spells...");
            foreach (var spell in RegulationParams.Magic)
            {
                ProcessSpell(spell);
            }

            Console.WriteLine($"Modifying item lots...");
            // Remove most items from the game
            foreach (var itemLot in RegulationParams.ItemLotParam_enemy)
            {
                ProcessItemLot(itemLot);
            }

            foreach (var itemLot in RegulationParams.ItemLotParam_map)
            {
                if (itemLot.Id == 2000 || itemLot.Id == 2001)
                {
                    // Flasks
                    continue;
                }
                
                var preserveItem = ProcessItemLot(itemLot);
                if (!preserveItem)
                {
                    RandomizeOneItemLot(itemLot, AllWeights, GoodWeaponTypes, true);
                }
            }

            // Add back in good items
            //RandomizeItemGroups(GameData.TeardropScarabs, ScarabWeights, GoodWeaponTypes);
            //RandomizeItemGroups(GameData.MajorBosses, MajorBossWeights, GoodWeaponTypes);
            //RandomizeItemGroups(GameData.MinorBosses, MinorBossWeights, GoodWeaponTypes);
            //RandomizeItemGroups(GameData.Churches, ChurchWeights, GoodWeaponTypes);
            //RandomizeItemGroups(GameData.Carriages, CarriageWeights, GoodWeaponTypes);
            //RandomizeItemGroups(GameData.MapPlinths, MapPlinthWeights, GoodWeaponTypes);
            //RandomizeItemGroups(GameData.GoldenTrees, GoldenTreeWeights, GoodWeaponTypes);
            //AddNiceStartingItems();
            GivePlayerMaxFlasks();

            ChosenItems.Clear(); // allow duplicates between world and shop.
            Console.WriteLine($"Modifying shop inventories...");
            var shuffledShopLineup = RegulationParams.ShopLineupParam.ToArray();
            Shuffle(shuffledShopLineup);
            foreach (var shopLineup in shuffledShopLineup)
            {
                ProcessShopLineup(shopLineup);
            }
            AddItemsToTwinMaidenHusks();

            ChosenItems.Clear(); // allow duplicates between world and starting classes.
            ModifyStartingGifts();
            ModifyStartingClasses();

            Console.WriteLine($"Saving regulation file...");
            RegulationParams.Save(ParamClassGenerator.RegulationOutPath);

            Console.WriteLine($"Done");
        }

        private static void ProcessSpell(MagicParam spell)
        {
            if (spell.RowName?.Length > 0)
            {
                spell.RequirementARC = 10;
                spell.RequirementFTH = 10;
                spell.RequirementINT = 10;
            }
        }

        private void AddNiceStartingItems()
        {
            var row = RegulationParams.ItemLotParam_map[18000080];
            row.ItemID1 = 1200; // Gold pickled fowl foot
            row.ItemCategory1 = ItemlotItemcategory.Good;

            row = RegulationParams.ItemLotParam_map[18000081];
            row.ItemID1 = 1110; // Gold scarab talisman
            row.ItemCategory1 = ItemlotItemcategory.Accessory;
        }

        private void RandomizeItemGroups(IEnumerable<ItemLotGroup> groups, ItemTypeAndWeight[] weights, WepType[] wepTypes, bool withReplacement = false)
        {
            var shuffledGroups = groups.ToArray();
            Shuffle(shuffledGroups);

            foreach (var group in shuffledGroups)
            {
                foreach (var itemLotId in group.ItemLotIds)
                {
                    var itemLot = RegulationParams.ItemLotParam_map[itemLotId];
                    RandomizeOneItemLot(itemLot, weights, wepTypes, withReplacement);
                }
            }
        }

        private void RandomizeOneItemLot(ItemLotParam itemLot, ItemTypeAndWeight[] weights, WepType[] wepTypes, bool withReplacement = false)
        {
            var item = GetRandomItemWeighted(weights, wepTypes, withReplacement);
            if (item != null)
            {
                itemLot.ItemID1 = item.Id;
                itemLot.ItemCategory1 = item.Category;
                itemLot.ItemChance1 = 1000;
                itemLot.ItemAmount1 = 1;
            }

            Console.WriteLine($"{itemLot.RowName}\n\tReplaced with {GetFriendlyItemName(item)}");
        }

        private string GetFriendlyItemName(ItemDescription desc)
        {
            if (desc == null)
            {
                return "nothing";
            }

            var name = GetRowFromItemDescription(desc, out int weaponUpgradeLevel).RowName;
            if (desc.Category == ItemlotItemcategory.Weapon)
            {
                name += $" +{weaponUpgradeLevel}";
            }

            return name;
        }

        private ParamRow GetRowFromItemDescription(ItemDescription desc, out int weaponUpgradeLevel)
        {
            weaponUpgradeLevel = 0;

            switch (desc.Category)
            {
                case ItemlotItemcategory.Accessory:
                    return RegulationParams.EquipParamAccessory[desc.Id];
                case ItemlotItemcategory.Good:
                    return RegulationParams.EquipParamGoods[desc.Id];
                case ItemlotItemcategory.Ash:
                    return RegulationParams.EquipParamGem[desc.Id];
                case ItemlotItemcategory.Protector:
                    return RegulationParams.EquipParamProtector[desc.Id];
                case ItemlotItemcategory.Weapon:
                    var id = desc.Id;
                    if (id.ToString().EndsWith("25"))
                    {
                        weaponUpgradeLevel = 25;
                    }
                    else if (id.ToString().EndsWith("10"))
                    {
                        weaponUpgradeLevel = 10;
                    }
                    return RegulationParams.EquipParamWeapon[desc.Id - weaponUpgradeLevel];
            }

            return null;
        }

        private void GivePlayerMaxFlasks()
        {
            // Give max flasks
            var row = RegulationParams.ItemLotParam_map[2000];
            row.ItemID1 = 1025;
            row.ItemAmount1 = 12;

            row = RegulationParams.ItemLotParam_map[2001];
            row.ItemID1 = 1075;
            row.ItemAmount1 = 2;
        }

        private bool IsKeyItemOrUniqueItem(int id)
        {
            var row = RegulationParams.EquipParamGoods[id];
            if (row != null)
            {
                return row.GoodsType == GoodsType.KeyItem || row.IsUniqueItem == EquipBool.True;
            }

            return false;
        }

        private void AddItemsToTwinMaidenHusks()
        {
            // Add whetstone and whetblades to twin maiden husks
            var row = RegulationParams.ShopLineupParam[101880];
            row.SellPriceOverwrite = 0;
            row.VisibilityEventFlagID = 0;

            row = RegulationParams.ShopLineupParam.AddNewRow(101882, "[Twin Maiden Husks] Iron Whetblade");
            row.SellPriceOverwrite = 0;
            row.ReferenceID = 8970;
            row.EquipmentType = ShopLineupEquiptype.Good;
            row.AmountToSell = 1;

            row = RegulationParams.ShopLineupParam.AddNewRow(101883, "[Twin Maiden Husks - Red-Hot Whetblade]");
            row.SellPriceOverwrite = 0;
            row.ReferenceID = 8971;
            row.EquipmentType = ShopLineupEquiptype.Good;
            row.AmountToSell = 1;

            row = RegulationParams.ShopLineupParam.AddNewRow(101884, "[Twin Maiden Husks - Sanctified Whetblade]");
            row.SellPriceOverwrite = 0;
            row.ReferenceID = 8972;
            row.EquipmentType = ShopLineupEquiptype.Good;
            row.AmountToSell = 1;

            row = RegulationParams.ShopLineupParam.AddNewRow(101885, "[Twin Maiden Husks - Glintstone Whetblade]");
            row.SellPriceOverwrite = 0;
            row.ReferenceID = 8973;
            row.EquipmentType = ShopLineupEquiptype.Good;
            row.AmountToSell = 1;

            row = RegulationParams.ShopLineupParam.AddNewRow(101886, "[Twin Maiden Husks - Black Whetblade]");
            row.SellPriceOverwrite = 0;
            row.ReferenceID = 8974;
            row.EquipmentType = ShopLineupEquiptype.Good;
            row.AmountToSell = 1;
        }

        private void ProcessShopLineup(ShopLineupParam shopLineup)
        {
            bool clearRow = true;
            if (GameData.ShopLineupIds.Contains(shopLineup.Id))
            {
                var item = GetRandomItemWeighted(ShopWeights, GoodWeaponTypes, false);
                Console.WriteLine($"{shopLineup.RowName}\n\tReplaced with {GetFriendlyItemName(item)}");
                if (item != null)
                {
                    clearRow = false;
                    shopLineup.ReferenceID = item.Id;
                    shopLineup.SellPriceOverwrite = -1;
                    shopLineup.RequiredMaterialID = -1;
                    shopLineup.QuantityEventFlagID = 0;
                    shopLineup.VisibilityEventFlagID = 0;
                    shopLineup.AmountToSell = 1;
                    shopLineup.EquipmentType = item.EquipType;
                    shopLineup.CurrencyType = ShopLineupCosttype.Runes;
                    shopLineup.AmountOnPurchase = 1;
                    shopLineup.PriceAddition = 0;
                    shopLineup.PriceMultiplier = 1;
                    shopLineup.IconID = -1;
                    shopLineup.NameTextID = -1;
                    shopLineup.MenuTitleTextID = -1;
                    shopLineup.MenuIconID = -1;
                }
            }

            if (clearRow)
            {
                shopLineup.ReferenceID = 0;
                shopLineup.QuantityEventFlagID = 0;
                shopLineup.VisibilityEventFlagID = 0;
                shopLineup.AmountToSell = 0;
            }
        }

        private void Shuffle<T>(T[] array)
        {
            int n = array.Length;
            for (int i = 0; i < (n - 1); i++)
            {
                // Use Next on random instance with an argument.
                // ... The argument is an exclusive bound.
                //     So we will not go past the end of the array.
                int r = i + RandomNumberGenerator.Next(n - i);
                T t = array[r];
                array[r] = array[i];
                array[i] = t;
            }
        }

        public enum ItemType
        {
            SpiritAsh,
            Talisman,
            PhysickTear,
            Sorcery,
            Incantation,
            Weapon,
            AshOfWar,
            Runes
        }

        private int ChooseRandomWeighted(float[] weights)
        {
            float totalWeight = weights.Sum();
            float random = (float)RandomNumberGenerator.NextDouble() * totalWeight;
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

        private ItemType? GetRandomItemTypeWeighted(IEnumerable<ItemTypeAndWeight> itemWeights, WepType[] wepTypes)
        {
            // Get array of item weights filtered by which item types still have items to be chosen
            var filtered = itemWeights.Where(item => GetRandomItemOfType(item.Type, wepTypes, true) != null).ToArray();
            var index = ChooseRandomWeighted(filtered.Select(item => item.Weight).ToArray());
            if (index >= 0 && index < filtered.Length)
            {
                return filtered[index].Type;
            }
            else
            {
                return null;
            }
        }

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

        // This is the API you want to use to get random items 99% of the time
        private ItemDescription GetRandomItemWeighted(ItemTypeAndWeight[] itemWeights, WepType[] wepTypes, bool withReplacement)
        {
            var itemType = GetRandomItemTypeWeighted(itemWeights, wepTypes);
            if (itemType != null)
            {
                return GetRandomItemOfType(itemType.Value, wepTypes, withReplacement || itemWeights.First(weight => weight.Type == itemType.Value).WithReplacement);
            }

            return null;
        }

        // This is the API you want to use to get random items 99% of the time
        private ItemDescription GetRandomItemOfType(ItemType type, bool withReplacement)
        {
            return GetRandomItemOfType(type, new WepType[] { }, withReplacement);
        }

        // This is the API you want to use to get random items 99% of the time
        private ItemDescription GetRandomItemOfType(ItemType type, WepType[] wepTypes, bool withReplacement)
        {
            ItemDescription output = null;
            switch (type)
            {
                case ItemType.Weapon:
                    output = new ItemDescription(GetRandomItemWithOrWithoutReplacement(GetPotentialWeaponIdsForTypes(wepTypes), ItemType.Weapon, withReplacement), ItemlotItemcategory.Weapon, ShopLineupEquiptype.Weapon);
                    break;
                case ItemType.Talisman:
                    output = new ItemDescription(GetRandomItemWithOrWithoutReplacement(GameData.Talismans, ItemType.Talisman, withReplacement), ItemlotItemcategory.Accessory, ShopLineupEquiptype.Accessory);
                    break;
                case ItemType.SpiritAsh:
                    output = new ItemDescription(GetRandomItemWithOrWithoutReplacement(GameData.SpiritAshIds, ItemType.SpiritAsh, withReplacement), ItemlotItemcategory.Good, ShopLineupEquiptype.Good);
                    break;
                case ItemType.Sorcery:
                    output = new ItemDescription(GetRandomItemWithOrWithoutReplacement(GameData.SorceryIds, ItemType.Sorcery, withReplacement), ItemlotItemcategory.Good, ShopLineupEquiptype.Good);
                    break;
                case ItemType.Incantation:
                    output = new ItemDescription(GetRandomItemWithOrWithoutReplacement(GameData.IncantationIds, ItemType.Incantation, withReplacement), ItemlotItemcategory.Good, ShopLineupEquiptype.Good);
                    break;
                case ItemType.PhysickTear:
                    output = new ItemDescription(GetRandomItemWithOrWithoutReplacement(GameData.Tears, ItemType.PhysickTear, withReplacement), ItemlotItemcategory.Good, ShopLineupEquiptype.Good);
                    break;
                case ItemType.AshOfWar:
                    output = new ItemDescription(GetRandomItemWithOrWithoutReplacement(GameData.AshOfWarIds, ItemType.AshOfWar, withReplacement), ItemlotItemcategory.Ash, ShopLineupEquiptype.Ashes);
                    break;
                case ItemType.Runes:
                    output = new ItemDescription(GetRandomItemWithOrWithoutReplacement(GameData.GoodRuneIds, ItemType.Runes, withReplacement), ItemlotItemcategory.Good, ShopLineupEquiptype.Good);
                    break;
            }

            if (output?.Id == -1)
            {
                return null;
            }

            return output;
        }

        // Return list of valid, fully upgraded weapons that match the specified types
        private IEnumerable<int> GetPotentialWeaponIdsForTypes(WepType[] wepTypes)
        {
            var typesHashset = new HashSet<WepType>(wepTypes);
            return RegulationParams.EquipParamWeapon.Where(weapon => weapon.RowName?.Length > 0 && typesHashset.Contains(weapon.WeaponType)).Select(weapon => WeaponMaxUpgradeIdDict[weapon]);
        }

        private int GetRandomItemWithOrWithoutReplacement(IEnumerable<int> potentialItemIds, ItemType type, bool withReplacement)
        {
            int[] actualItemIds = null;
            // Even if you say with replacement, don't include items that have already been taken out
            if (!ChosenItems.ContainsKey(type))
            {
                ChosenItems.Add(type, new HashSet<int>());
            }

            var chosenItemHashset = ChosenItems[type];
            actualItemIds = potentialItemIds.Where(id => !chosenItemHashset.Contains(id)).ToArray();

            if (actualItemIds.Length > 0)
            {
                var index = RandomNumberGenerator.Next(0, actualItemIds.Length);
                var itemId = actualItemIds[index];

                if (!withReplacement)
                {
                    ChosenItems[type].Add(itemId);
                }

                return itemId;
            }

            return -1;
        }

        private void ModifyStartingGifts()
        {
            Console.WriteLine($"Modifying starting gifts...");
            for (int giftId = 2400; giftId <= 2409; giftId++)
            {
                var gift = RegulationParams.CharaInitParam[giftId];
                gift.EquippedAccessorySlot1 = -1;
                gift.EquippedItemSlot3 = -1;
                gift.EquippedItemSlot3Amount = 0;
            }
        }

        private void ModifyStartingClasses()
        {
            Console.WriteLine($"Modifying starting classes...");
            for (int classId = 3000; classId <= 3009; classId++)
            {
                var startingClass = RegulationParams.CharaInitParam[classId];

                startingClass.EquippedAccessorySlot1 = GetRandomItemOfType(ItemType.Talisman, false).Id;
                startingClass.EquippedAccessorySlot2 = GetRandomItemOfType(ItemType.Talisman, false).Id;

                startingClass.EquippedWeaponRightPrimary = -1;
                startingClass.EquippedWeaponRightSecondary = -1;
                startingClass.EquippedWeaponRightTertiary = -1;
                startingClass.EquippedWeaponLeftPrimary = -1;
                startingClass.EquippedWeaponLeftSecondary = -1;
                startingClass.EquippedWeaponLeftTertiary = -1;

                // Wretch stats
                startingClass.Level = 1;
                startingClass.Vigor = 30;
                startingClass.Attunement = 20;
                startingClass.Endurance = 10;
                startingClass.Strength = 10;
                startingClass.Dexterity = 10;
                startingClass.Intelligence = 10;
                startingClass.Faith = 10;
                startingClass.Arcane = 10;

                // Lantern
                startingClass.EquippedItemSlot1 = 2070;
                startingClass.EquippedItemSlot1Amount = 1;

                // Stonesword key
                startingClass.EquippedItemSlot2 = 8000;
                startingClass.EquippedItemSlot2Amount = 10;

                // Spirit ashes
                // TODO - consider making this only give bad spirit ashes
                startingClass.EquippedItemSlot3 = GetRandomItemOfType(ItemType.SpiritAsh, false).Id;
                startingClass.EquippedItemSlot3Amount = 1;

                // Physick Tears
                startingClass.EquippedItemSlot4 = GetRandomItemOfType(ItemType.PhysickTear, false).Id;
                startingClass.EquippedItemSlot4Amount = 1;

                startingClass.EquippedItemSlot5 = GetRandomItemOfType(ItemType.PhysickTear, false).Id;
                startingClass.EquippedItemSlot5Amount = 1;

                // Spirit calling bell
                startingClass.EquippedItemSlot6 = 8158;
                startingClass.EquippedItemSlot6Amount = 1;

                // Talisman pouches
                startingClass.EquippedItemSlot7 = 10040;
                startingClass.EquippedItemSlot7Amount = 3;

                // Memory stones
                startingClass.EquippedItemSlot8 = 10030;
                startingClass.EquippedItemSlot8Amount = 9;

                // Flash of wondrous physick
                startingClass.StoredItemSlot3 = 251;
                startingClass.StoredItemSlot3Count = 1;

                // Spectral steed whistle
                startingClass.StoredItemSlot4 = 130;
                startingClass.StoredItemSlot4Count = 1;

                startingClass.HPFlaskMaxPossessionLimit = 12;
                startingClass.FPFlaskMaxPossessionLimit = 2;
            }

            var allMeleeWeapons = new WepType[] {
                WepType.Dagger,
                WepType.StraightSword,
                WepType.Greatsword,
                WepType.ColossalSword,
                WepType.CurvedSword,
                WepType.CurvedGreatsword,
                WepType.Katana,
                WepType.Twinblade,
                WepType.ThrustingSword,
                WepType.HeavyThrustingSword,
                WepType.Axe,
                WepType.Greataxe,
                WepType.Hammer,
                WepType.GreatHammer,
                WepType.Flail,
                WepType.Spear,
                WepType.HeavySpear,
                WepType.Halberd,
                WepType.Scythe,
                WepType.Fist,
                WepType.Claw,
                WepType.Whip,
                WepType.ColossalWeapon
            };

            var moreStrBasedMeleeWeapons = new WepType[] {
                WepType.StraightSword,
                WepType.Greatsword,
                WepType.ColossalSword,
                WepType.Axe,
                WepType.Greataxe,
                WepType.Hammer,
                WepType.GreatHammer,
                WepType.Spear,
                WepType.HeavySpear,
                WepType.Halberd,
                WepType.Scythe,
                WepType.Fist,
                WepType.ColossalWeapon
            };

            var moreDexBasedMeleeWeapons = new WepType[]
            {
                WepType.Dagger,
                WepType.CurvedSword,
                WepType.CurvedGreatsword,
                WepType.Katana,
                WepType.Twinblade,
                WepType.ThrustingSword,
                WepType.HeavyThrustingSword,
                WepType.Flail,
                WepType.Claw,
                WepType.Whip
            };

            var shields = new WepType[] {
                WepType.SmallShield,
                WepType.MediumShield,
                WepType.Greatshield
            };

            // Vagabond
            PickStartingClassWeapons(RegulationParams.CharaInitParam[3000], allMeleeWeapons, shields);
            // Warrior
            PickStartingClassWeapons(RegulationParams.CharaInitParam[3001], moreDexBasedMeleeWeapons, shields);
            // Hero
            PickStartingClassWeapons(RegulationParams.CharaInitParam[3002], moreStrBasedMeleeWeapons, shields);
            // Bandit
            PickStartingClassWeapons(RegulationParams.CharaInitParam[3003], moreDexBasedMeleeWeapons, shields);
            // Astrologer
            PickStartingClassWeapons(RegulationParams.CharaInitParam[3004], allMeleeWeapons, shields, WepType.Staff);
            // Prophet
            PickStartingClassWeapons(RegulationParams.CharaInitParam[3005], allMeleeWeapons, shields, WepType.Seal);
            // Confessor
            PickStartingClassWeapons(RegulationParams.CharaInitParam[3006], allMeleeWeapons, shields, WepType.Seal);
            // Samurai
            PickStartingClassWeapons(RegulationParams.CharaInitParam[3007], moreDexBasedMeleeWeapons, shields);
            // Prisoner
            PickStartingClassWeapons(RegulationParams.CharaInitParam[3008], allMeleeWeapons, shields, WepType.Staff);
            // Wretch
            PickStartingClassWeapons(RegulationParams.CharaInitParam[3009], allMeleeWeapons, shields);
        }

        void PickStartingClassWeapons(CharaInitParam startingClass, WepType[] rightHand, WepType[] leftHand, WepType guaranteedRightHand = WepType.None)
        {
            var howManyRight = 3;
            var howManyLeft = 1;
            string[] slots = new string[] { "Primary", "Secondary", "Tertiary" };

            for (int i = 0; i < howManyRight; i++)
            {
                var types = rightHand;
                if (i == 0 && guaranteedRightHand != WepType.None)
                {
                    types = new WepType[] { guaranteedRightHand };
                }
                var weaponId = GetRandomItemOfType(ItemType.Weapon, types, false).Id;
                startingClass[$"EquippedWeaponRight{slots[i]}"].Value = weaponId;
            }

            for (int i = 0; i < howManyLeft; i++)
            {
                var weaponId = GetRandomItemOfType(ItemType.Weapon, leftHand, false).Id;
                startingClass[$"EquippedWeaponLeft{slots[i]}"].Value = weaponId;
            }

            if (guaranteedRightHand == WepType.Staff || guaranteedRightHand == WepType.Seal)
            {
                for (int i = 0; i < 4; i++)
                {
                    var spellId = GetRandomItemOfType(guaranteedRightHand == WepType.Staff ? ItemType.Sorcery : ItemType.Incantation, false).Id;
                    startingClass[$"EquippedSpellSlot{i + 1}"].Value = spellId;
                }
            }
        }

        // Gather info about max upgrade level for the weapon, and remove its starting requirements
        void ProcessWeapon(EquipParamWeapon weapon)
        {
            weapon.RequirementSTR = 10;
            weapon.RequirementDEX = 10;
            weapon.RequirementFTH = 10;
            weapon.RequirementINT = 10;
            weapon.RequirementARC = 10;
            weapon.Weight = 0;

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
        bool ProcessItemLot(ItemLotParam itemLot)
        {
            bool preserve = false;
            for (int i = 1; i <= 8; i++)
            {
                var itemIdCell = itemLot[$"ItemID{i}"];
                var categoryCell = itemLot[$"ItemCategory{i}"];
                var amountCell = itemLot[$"ItemAmount{i}"];
                var itemChanceCell = itemLot[$"ItemChance{i}"];

                var itemId = (int)itemIdCell.Value;
                var category = (ItemlotItemcategory)categoryCell.Value;

                // Key item, unique item, or serpent-hunter
                if ((category == ItemlotItemcategory.Good && IsKeyItemOrUniqueItem(itemId)) || (category == ItemlotItemcategory.Weapon && itemId == 17030000))
                {
                    preserve = true;
                    continue;
                }

                itemIdCell.Value = 0;
                itemChanceCell.Value = 0;
                amountCell.Value = 0;
                categoryCell.Value = ItemlotItemcategory.None;
            }

            return preserve;
        }
    }
}
#endif