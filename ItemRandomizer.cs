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
        private int CurrentTaskIndex = 0;
        private ItemRandomizerHelper Helper;
        private string RegulationInPath;
        private string RegulationOutPath;
        public List<string> SpoilerLog;

        public ItemRandomizer(ItemRandomizerParams options, string regulationInPath, string regulationOutPath)
        {
            Options = options;
            RegulationInPath = regulationInPath;
            RegulationOutPath = regulationOutPath;
            SpoilerLog = new List<string>();
        }

        private static TaskDefinition[] Tasks = new TaskDefinition[] {
            new TaskDefinition("Loading regulation file"),
            new TaskDefinition("Modifying weapons"),
            new TaskDefinition("Modifying armor"),
            new TaskDefinition("Modifying spells"),
            new TaskDefinition("Modifying reinforce params"),
            new TaskDefinition("Generating game state"),
            new TaskDefinition("Removing enemy items"),
            new TaskDefinition("Randomizing map items"),
            new TaskDefinition("Randomizing shop items"),
            new TaskDefinition("Randomizing starting classes"),
            new TaskDefinition("Saving regulation file"),
        };

        public void Run()
        {
            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            ChosenItems = new Dictionary<ItemType, HashSet<int>>();
            RandomNumberGenerator = new Random(Options.Seed);
            CurrentTaskIndex = -1;

            LoadRegulationFile();
            ModifyWeapons();
            Helper = new ItemRandomizerHelper(RandomNumberGenerator, GameData, RegulationParams, WeaponMaxUpgradeIdDict);
            ModifyArmor();
            ModifySpells();
            ModifyReinforceParams();
            GenerateGameState();
            RemoveEnemyItemLots();
            RandomizeMapItemLots();
            RandomizeShopItems();
            RandomizeStartingClasses();
            SaveRegulationFile();

            stopwatch.Stop();

            OnProgressChanged(1, $"Done. Took {stopwatch.ElapsedMilliseconds / 1000f:F2}s");
        }

        private void UpdateProgress(float taskProgress)
        {
            OnProgressChanged((CurrentTaskIndex + taskProgress) / Tasks.Length, Tasks[CurrentTaskIndex].Name);
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
        private RandomizerGameState RandomizedGameState;

        private void LoadRegulationFile()
        {
            CurrentTaskIndex++;
            UpdateProgress(0);
            RegulationParams = RegulationParams.Load(RegulationInPath);
            GameData = new GameData(RegulationParams);
        }

        private void ModifyWeapons()
        {
            CurrentTaskIndex++;
            UpdateProgress(0);
            var weapons = RegulationParams.EquipParamWeapon.ToArray();
            int count = weapons.Count();
            for (int i = 0; i < count; i++)
            {
                UpdateProgress(i / (float)count);
                ProcessWeapon(weapons[i]);
            }
        }

        private void ModifyArmor()
        {
            CurrentTaskIndex++;
            UpdateProgress(0);
            var armor = RegulationParams.EquipParamProtector.ToArray();
            int count = armor.Count();
            for (int i = 0; i < count; i++)
            {
                UpdateProgress(i / (float)count);
                ProcessArmor(armor[i]);
            }

            Helper = new ItemRandomizerHelper(RandomNumberGenerator, GameData, RegulationParams, WeaponMaxUpgradeIdDict);
        }

        private void ModifySpells()
        {
            CurrentTaskIndex++;
            UpdateProgress(0);
            var spells = RegulationParams.Magic.ToArray();
            int count = spells.Count();
            for (int i = 0; i < count; i++)
            {
                UpdateProgress(i / (float)count);
                ProcessSpell(spells[i]);
            }
        }

        private void ModifyReinforceParams()
        {
            CurrentTaskIndex++;
            UpdateProgress(0);
            var reinforceParamWeapons = RegulationParams.ReinforceParamWeapon.ToArray();
            int count = reinforceParamWeapons.Count();
            for (int i = 0; i < count; i++)
            {
                UpdateProgress(i / (float)count);
                ProcessReinforceParamWeapon(reinforceParamWeapons[i]);
            }
        }

        private void GenerateGameState()
        {
            CurrentTaskIndex++;
            UpdateProgress(0);

            var allBossIndices = GameData.RandomizedBosses.Select((_, index) => index).ToArray();
            RandomUtils.Shuffle(RandomNumberGenerator, ref allBossIndices);

            var allGreatRuneIndices = GameData.GreatRunes.Select((_, index) => index).ToArray();
            RandomUtils.Shuffle(RandomNumberGenerator, ref allGreatRuneIndices);

            var bossIndices = allBossIndices.Where(index => ShouldConsiderBoss(GameData.RandomizedBosses[index])).Take(Options.GreatRunesRequired).ToArray();
            var greatRuneIndices = allGreatRuneIndices.Take(Options.GreatRunesRequired).ToArray();
            var pairs = bossIndices.Select((bossIndex, bossArrayIndex) => new Tuple<int, int>(bossIndex, greatRuneIndices[bossArrayIndex])).ToArray();

            RandomizedGameState = new RandomizerGameState()
            {
                BossDefinitionGreatRunePairs = pairs
            };

            // Save it for later
            JSON.SaveToFile("state.json", RandomizedGameState);
        }

        private bool ShouldConsiderBoss(BossDefinition definition)
        {
            return definition.Type switch
            {
                BossType.Legends => Options.GreatRunesFromBossLegend,
                BossType.GreatEnemies => Options.GreatRunesFromBossGreatEnemy,
                BossType.FieldBosses => Options.GreatRunesFromBossField,
            };
        }

        private void RemoveEnemyItemLots()
        {
            CurrentTaskIndex++;
            UpdateProgress(0);
            var enemyItemLots = RegulationParams.ItemLotParam_enemy.ToArray();
            int count = enemyItemLots.Count();
            for (int i = 0; i < count; i++)
            {
                UpdateProgress(i / (float)count);
                ProcessItemLot(enemyItemLots[i]);
            }
        }

        private void RandomizeMapItemLots()
        {
            CurrentTaskIndex++;
            UpdateProgress(0);
            var mapItemLots = RegulationParams.ItemLotParam_map.ToArray();
            int count = mapItemLots.Count();
            var addlParams = new ItemAdditionalParams(GoodWeaponTypes, new ProtectorCategory[] { });
            for (int i = 0; i < count; i++)
            {
                UpdateProgress(i / (float)count);
                var itemLot = mapItemLots[i];

                if (!ShouldPreserveMapItemLot(itemLot))
                {
                    // Remove all items in this lot, and replace with random stuff
                    ProcessItemLot(itemLot);
                    ReplaceMapItemLot(itemLot, addlParams);
                }
            }
        }

        private void RandomizeShopItems()
        {
            CurrentTaskIndex++;
            UpdateProgress(0);
            var shuffledShopLineup = RegulationParams.ShopLineupParam.ToArray();
            RandomUtils.Shuffle(RandomNumberGenerator, ref shuffledShopLineup);
            int count = shuffledShopLineup.Count();
            for (int i = 0; i < count; i++)
            {
                UpdateProgress(i / (float)count);
                ProcessShopLineup(shuffledShopLineup[i]);
            }

            AddItemsToTwinMaidenHusks();
        }

        private void RandomizeStartingClasses()
        {
            CurrentTaskIndex++;
            UpdateProgress(0);
            ChosenItems.Clear(); // allow duplicates between world and starting classes.
            ModifyStartingGifts();
            for (int classId = 3000; classId <= 3009; classId++)
            {
                UpdateProgress((classId - 3000) / 10.0f);
                var startingClass = RegulationParams.CharaInitParam[classId];

                startingClass.EquippedAccessorySlot1 = Helper.GetRandomItemOfType(ItemType.Talisman, null, false).Id;
                startingClass.EquippedAccessorySlot2 = Helper.GetRandomItemOfType(ItemType.Talisman, null, false).Id;
                startingClass.EquippedAccessorySlot3 = Helper.GetRandomItemOfType(ItemType.Talisman, null, false).Id;
                startingClass.EquippedAccessorySlot4 = Helper.GetRandomItemOfType(ItemType.Talisman, null, false).Id;

                startingClass.EquippedWeaponRightPrimary = -1;
                startingClass.EquippedWeaponRightSecondary = -1;
                startingClass.EquippedWeaponRightTertiary = -1;
                startingClass.EquippedWeaponLeftPrimary = -1;
                startingClass.EquippedWeaponLeftSecondary = -1;
                startingClass.EquippedWeaponLeftTertiary = -1;

                startingClass.EquippedArmorArms = -1;
                startingClass.EquippedArmorChest = -1;
                startingClass.EquippedArmorHead = -1;
                startingClass.EquippedArmorLegs = -1;

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

            for (int id = 3000; id <= 3009; id++)
            {
                PickStartingClassWeapons(RegulationParams.CharaInitParam[id], allMeleeWeapons, allMeleeWeapons);
                PickStartingClassArmor(RegulationParams.CharaInitParam[id]);
            }
        }

        private void PickStartingClassArmor(CharaInitParam charaInitParam)
        {
            charaInitParam.EquippedArmorHead = Helper.GetRandomItemOfType(ItemType.Armor, new ItemAdditionalParams(null, new ProtectorCategory[] { ProtectorCategory.Head }), false).Id;
            charaInitParam.EquippedArmorChest = Helper.GetRandomItemOfType(ItemType.Armor, new ItemAdditionalParams(null, new ProtectorCategory[] { ProtectorCategory.Body }), false).Id;
            charaInitParam.EquippedArmorArms = Helper.GetRandomItemOfType(ItemType.Armor, new ItemAdditionalParams(null, new ProtectorCategory[] { ProtectorCategory.Arms }), false).Id;
            charaInitParam.EquippedArmorLegs = Helper.GetRandomItemOfType(ItemType.Armor, new ItemAdditionalParams(null, new ProtectorCategory[] { ProtectorCategory.Legs }), false).Id;
        }

        private void SaveRegulationFile()
        {
            CurrentTaskIndex++;
            UpdateProgress(0);
            RegulationParams.Save(RegulationOutPath);
        }

        private void ProcessArmor(EquipParamProtector equipParamProtector)
        {
            equipParamProtector.Weight = 0;
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

        private bool ShouldPreserveMapItemLot(ItemLotParam mapLot)
        {
            if (mapLot.RowName?.Length > 0 && mapLot.RowName.StartsWith("[Material]"))
            {
                return true; // Preserve crafting materials
            }

            return mapLot.Id switch
            {
                16000690 => true, // Serpent-Hunter
                _ => false
            };
        }

        private void ReplaceMapItemLot(ItemLotParam itemLot, ItemAdditionalParams addlParams)
        {
            // Place great runes in boss drops
            var pair = Array.Find(RandomizedGameState.BossDefinitionGreatRunePairs, element => GameData.RandomizedBosses[element.Item1].MapItemLotIds[0] == itemLot.Id);
            if (pair != null)
            {
                var bossDefinition = GameData.RandomizedBosses[pair.Item1];
                var greatRune = GameData.GreatRunes[pair.Item2];
                itemLot.ItemID1 = greatRune.Id;
                itemLot.ItemCategory1 = greatRune.Category;
                itemLot.ItemChance1 = 1000;
                itemLot.ItemAmount1 = 1;
                itemLot.ItemAcquisitionFlag = (uint)greatRune.EventId;

                SpoilerLog.Add($"Required: {bossDefinition.Name} (drops {greatRune.Name})");

                return;
            }

            RandomizeOneItemLot(itemLot, AllWeights, addlParams, true);
        }

        private void AddItemsToTwinMaidenHusks()
        {
            // TODO Remove
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
        void ProcessItemLot(ItemLotParam itemLot)
        {
            for (int i = 1; i <= 8; i++)
            {
                var itemIdCell = itemLot[$"ItemID{i}"];
                var categoryCell = itemLot[$"ItemCategory{i}"];
                var amountCell = itemLot[$"ItemAmount{i}"];
                var itemChanceCell = itemLot[$"ItemChance{i}"];
                var itemAcquisitionFlagCell = itemLot[$"ItemAcquisitionFlag{i}"];

                itemIdCell.Value = 0;
                itemChanceCell.Value = 0;
                amountCell.Value = 0;
                categoryCell.Value = ItemlotItemcategory.None;
                itemAcquisitionFlagCell.Value = 0; // TODO: necessary?
            }

            itemLot.ItemAcquisitionFlag = 0; // TODO: necessary?
        }
    }
}
#endif