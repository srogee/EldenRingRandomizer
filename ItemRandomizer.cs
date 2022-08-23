//#define GenerateCode

#if GenerateCode
#else

using StronglyTypedParams;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EldenRingItemRandomizer
{
    struct TaskDefinition
    {
        public string Name;
        public float OverallPercentage;

        public TaskDefinition(string name, float overallPercentage = 0)
        {
            Name = name;
            OverallPercentage = overallPercentage;
        }
    }

    class ItemRandomizer
    {
        private ItemRandomizerHelper Helper;
        private string RegulationInPath;
        private string RegulationOutPath;

        public ItemRandomizer(ItemRandomizerParams options, string regulationInPath, string regulationOutPath)
        {
            Options = options;
            RegulationInPath = regulationInPath;
            RegulationOutPath = regulationOutPath;
        }

        private static TaskDefinition[] Tasks = new TaskDefinition[] {
            new TaskDefinition("Loading regulation file"),
            new TaskDefinition("Modifying weapons"),
            new TaskDefinition("Modifying spells"),
            new TaskDefinition("Modifying reinforce params"),
            new TaskDefinition("Removing enemy items"),
            new TaskDefinition("Randomizing map items"),
            new TaskDefinition("Randomizing shop items"),
            new TaskDefinition("Randomizing starting classes"),
            new TaskDefinition("Saving regulation file"),
        };

        public void Run()
        {
            ChosenItems = new Dictionary<ItemType, HashSet<int>>();
            RandomNumberGenerator = new Random(Options.Seed);
            var stopwatch = new System.Diagnostics.Stopwatch();

            stopwatch.Start();
            LoadRegulationFile();
            ModifyWeapons();
            ModifySpells();
            ModifyReinforceParams();
            RemoveEnemyItemLots();
            RandomizeMapItemLots();
            RandomizeShopItems();
            RandomizeStartingClasses();
            SaveRegulationFile();
            stopwatch.Stop();

            OnProgressChanged(1, $"Done. Took {stopwatch.ElapsedMilliseconds / 1000f:F2}s");
        }

        private void UpdateProgress(int taskIndex, float taskProgress)
        {
            OnProgressChanged((taskIndex + taskProgress) / Tasks.Length, Tasks[taskIndex].Name);
        }

        public delegate void ProgressChangedEvent(float precent, string message);
        public ProgressChangedEvent OnProgressChanged;

        private ItemRandomizerParams Options;

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

        private void LoadRegulationFile()
        {
            int taskIndex = 0;
            UpdateProgress(taskIndex, 0);
            RegulationParams = RegulationParams.Load(RegulationInPath);
            GameData = new GameData(RegulationParams);
        }

        private void ModifyWeapons()
        {
            int taskIndex = 1;
            UpdateProgress(taskIndex, 0);
            var weapons = RegulationParams.EquipParamWeapon.ToArray();
            int count = weapons.Count();
            for (int i = 0; i < count; i++)
            {
                UpdateProgress(taskIndex, i / (float)count);
                ProcessWeapon(weapons[i]);
            }

            Helper = new ItemRandomizerHelper(RandomNumberGenerator, GameData, RegulationParams, WeaponMaxUpgradeIdDict);
        }

        private void ModifySpells()
        {
            int taskIndex = 2;
            UpdateProgress(taskIndex, 0);
            var spells = RegulationParams.Magic.ToArray();
            int count = spells.Count();
            for (int i = 0; i < count; i++)
            {
                UpdateProgress(taskIndex, i / (float)count);
                ProcessSpell(spells[i]);
            }
        }

        private void ModifyReinforceParams()
        {
            int taskIndex = 3;
            UpdateProgress(taskIndex, 0);
            var reinforceParamWeapons = RegulationParams.ReinforceParamWeapon.ToArray();
            int count = reinforceParamWeapons.Count();
            for (int i = 0; i < count; i++)
            {
                UpdateProgress(taskIndex, i / (float)count);
                ProcessReinforceParamWeapon(reinforceParamWeapons[i]);
            }
        }

        private void RemoveEnemyItemLots()
        {
            int taskIndex = 4;
            UpdateProgress(taskIndex, 0);
            var enemyItemLots = RegulationParams.ItemLotParam_enemy.ToArray();
            int count = enemyItemLots.Count();
            for (int i = 0; i < count; i++)
            {
                UpdateProgress(taskIndex, i / (float)count);
                ProcessItemLot(enemyItemLots[i]);
            }
        }

        private void RandomizeMapItemLots()
        {
            int taskIndex = 5;
            UpdateProgress(taskIndex, 0);
            var mapItemLots = RegulationParams.ItemLotParam_map.ToArray();
            int count = mapItemLots.Count();
            var addlParams = new ItemAdditionalParams(GoodWeaponTypes, new ProtectorCategory[] { });
            for (int i = 0; i < count; i++)
            {
                UpdateProgress(taskIndex, i / (float)count);
                var itemLot = mapItemLots[i];
                if (itemLot.Id == 2000 || itemLot.Id == 2001)
                {
                    // Flasks
                    continue;
                }

                var preserveItem = ProcessItemLot(itemLot);
                if (!preserveItem)
                {
                    RandomizeOneItemLot(itemLot, AllWeights, addlParams, true);
                }
            }

            GivePlayerMaxFlasks();
        }

        private void RandomizeShopItems()
        {
            int taskIndex = 6;
            UpdateProgress(taskIndex, 0);
            var shuffledShopLineup = RegulationParams.ShopLineupParam.ToArray();
            Shuffle(shuffledShopLineup);
            int count = shuffledShopLineup.Count();
            for (int i = 0; i < count; i++)
            {
                UpdateProgress(taskIndex, i / (float)count);
                ProcessShopLineup(shuffledShopLineup[i]);
            }

            AddItemsToTwinMaidenHusks();
        }

        private void RandomizeStartingClasses()
        {
            int taskIndex = 7;
            UpdateProgress(taskIndex, 0);
            ChosenItems.Clear(); // allow duplicates between world and starting classes.
            ModifyStartingGifts();
            for (int classId = 3000; classId <= 3009; classId++)
            {
                UpdateProgress(taskIndex, (classId - 3000) / 10.0f);
                var startingClass = RegulationParams.CharaInitParam[classId];

                startingClass.EquippedAccessorySlot1 = Helper.GetRandomItemOfType(ItemType.Talisman, null, false).Id;
                startingClass.EquippedAccessorySlot2 = Helper.GetRandomItemOfType(ItemType.Talisman, null, false).Id;

                startingClass.EquippedWeaponRightPrimary = -1;
                startingClass.EquippedWeaponRightSecondary = -1;
                startingClass.EquippedWeaponRightTertiary = -1;
                startingClass.EquippedWeaponLeftPrimary = -1;
                startingClass.EquippedWeaponLeftSecondary = -1;
                startingClass.EquippedWeaponLeftTertiary = -1;

                // Wretch stats
                startingClass.Level = 1;
                startingClass.Vigor = 30;
                startingClass.Attunement = 10;
                startingClass.Endurance = 10;
                startingClass.Strength = 10;
                startingClass.Dexterity = 10;
                startingClass.Intelligence = 10;
                startingClass.Faith = 10;
                startingClass.Arcane = 10;

                // Lantern
                startingClass.EquippedItemSlot1 = 2070;
                startingClass.EquippedItemSlot1Amount = 1;

                // Remove spells
                startingClass.EquippedSpellSlot1 = -1;
                startingClass.EquippedSpellSlot2 = -1;
                startingClass.EquippedSpellSlot3 = -1;
                startingClass.EquippedSpellSlot4 = -1;
                startingClass.EquippedSpellSlot5 = -1;
                startingClass.EquippedSpellSlot6 = -1;
                startingClass.EquippedSpellSlot7 = -1;

                // Talisman pouches
                startingClass.EquippedItemSlot7 = 10040;
                startingClass.EquippedItemSlot7Amount = 3;

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

            // Currently, all the same
            // Vagabond
            PickStartingClassWeapons(RegulationParams.CharaInitParam[3000], allMeleeWeapons, allMeleeWeapons);
            // Warrior
            PickStartingClassWeapons(RegulationParams.CharaInitParam[3001], allMeleeWeapons, allMeleeWeapons);
            // Hero
            PickStartingClassWeapons(RegulationParams.CharaInitParam[3002], allMeleeWeapons, allMeleeWeapons);
            // Bandit
            PickStartingClassWeapons(RegulationParams.CharaInitParam[3003], allMeleeWeapons, allMeleeWeapons);
            // Astrologer
            PickStartingClassWeapons(RegulationParams.CharaInitParam[3004], allMeleeWeapons, allMeleeWeapons);
            // Prophet
            PickStartingClassWeapons(RegulationParams.CharaInitParam[3005], allMeleeWeapons, allMeleeWeapons);
            // Confessor
            PickStartingClassWeapons(RegulationParams.CharaInitParam[3006], allMeleeWeapons, allMeleeWeapons);
            // Samurai
            PickStartingClassWeapons(RegulationParams.CharaInitParam[3007], allMeleeWeapons, allMeleeWeapons);
            // Prisoner
            PickStartingClassWeapons(RegulationParams.CharaInitParam[3008], allMeleeWeapons, allMeleeWeapons);
            // Wretch
            PickStartingClassWeapons(RegulationParams.CharaInitParam[3009], allMeleeWeapons, allMeleeWeapons);
        }

        private void SaveRegulationFile()
        {
            int taskIndex = 8;
            UpdateProgress(taskIndex, 0);
            RegulationParams.Save(RegulationOutPath);
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

        private void RandomizeItemGroups(IEnumerable<ItemLotGroup> groups, ItemTypeAndWeight[] weights, ItemAdditionalParams addlParams, bool withReplacement = false)
        {
            var shuffledGroups = groups.ToArray();
            Shuffle(shuffledGroups);

            foreach (var group in shuffledGroups)
            {
                foreach (var itemLotId in group.ItemLotIds)
                {
                    var itemLot = RegulationParams.ItemLotParam_map[itemLotId];
                    RandomizeOneItemLot(itemLot, weights, addlParams, withReplacement);
                }
            }
        }

        private void RandomizeOneItemLot(ItemLotParam itemLot, ItemTypeAndWeight[] weights, ItemAdditionalParams addlParams, bool withReplacement = false)
        {
            var item = Helper.GetRandomItemWeighted(weights, addlParams, withReplacement);
            if (item != null)
            {
                itemLot.ItemID1 = item.Id;
                itemLot.ItemCategory1 = item.Category;
                itemLot.ItemChance1 = 1000;
                itemLot.ItemAmount1 = 1;
            }

            //Console.WriteLine($"{itemLot.RowName}\n\tReplaced with {GetFriendlyItemName(item)}");
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
                var item = Helper.GetRandomItemWeighted(ShopWeights, new ItemAdditionalParams(GoodWeaponTypes, new ProtectorCategory[] { }), false);
                //Console.WriteLine($"{shopLineup.RowName}\n\tReplaced with {GetFriendlyItemName(item)}");
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

        private void ModifyStartingGifts()
        {
            //Console.WriteLine($"Modifying starting gifts...");
            for (int giftId = 2400; giftId <= 2409; giftId++)
            {
                var gift = RegulationParams.CharaInitParam[giftId];
                gift.EquippedAccessorySlot1 = -1;
                gift.EquippedItemSlot3 = -1;
                gift.EquippedItemSlot3Amount = 0;
            }
        }

        void PickStartingClassWeapons(CharaInitParam startingClass, WepType[] rightHand, WepType[] leftHand, WepType guaranteedRightHand = WepType.None)
        {
            var howManyRight = 3;
            var howManyLeft = 3;
            string[] slots = new string[] { "Primary", "Secondary", "Tertiary" };

            for (int i = 0; i < howManyRight; i++)
            {
                var types = rightHand;
                if (i == 0 && guaranteedRightHand != WepType.None)
                {
                    types = new WepType[] { guaranteedRightHand };
                }
                var weaponId = Helper.GetRandomItemOfType(ItemType.Weapon, new ItemAdditionalParams(types, new ProtectorCategory[] { }), false).Id;
                startingClass[$"EquippedWeaponRight{slots[i]}"].Value = weaponId;
            }

            for (int i = 0; i < howManyLeft; i++)
            {
                var weaponId = Helper.GetRandomItemOfType(ItemType.Weapon, new ItemAdditionalParams(leftHand, new ProtectorCategory[] { }), false).Id;
                startingClass[$"EquippedWeaponLeft{slots[i]}"].Value = weaponId;
            }

            if (guaranteedRightHand == WepType.Staff || guaranteedRightHand == WepType.Seal)
            {
                for (int i = 0; i < 4; i++)
                {
                    var spellId = Helper.GetRandomItemOfType(guaranteedRightHand == WepType.Staff ? ItemType.Sorcery : ItemType.Incantation, null, false).Id;
                    startingClass[$"EquippedSpellSlot{i + 1}"].Value = spellId;
                }
            }
        }

        void ProcessReinforceParamWeapon(ReinforceParamWeapon reinforceParamWeapon)
        {
            var damagePercentNames = new string[] { "DamagePercentPhysical", "DamagePercentMagic", "DamagePercentFire", "DamagePercentHoly", "DamagePercentLightning" };
            var scalingPercentNames = new string[] { "CorrectionPercentSTR", "CorrectionPercentARC", "CorrectionPercentDEX", "CorrectionPercentINT", "CorrectionPercentFTH" };

            foreach (var name in damagePercentNames)
            {
                reinforceParamWeapon[name].Value = (float)reinforceParamWeapon[name].Value * Options.WeaponBaseDamageMultiplier; 
            }

            foreach (var name in scalingPercentNames)
            {
                reinforceParamWeapon[name].Value = (float)reinforceParamWeapon[name].Value * Options.WeaponScalingMultiplier;
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