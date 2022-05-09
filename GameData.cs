using StronglyTypedParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EldenRingItemRandomizer
{
    public class ItemLotGroup
    {
        public List<int> ItemLotIds;
        public ItemLotGroup(params int[] itemLotIds)
        {
            ItemLotIds = new List<int>(itemLotIds);
        }
    }

    class GameData
    {
        private RegulationParams RegulationParams;

        public int[] SpiritAshIds { get; }
        public int[] Talismans { get; }
        public int[] Tears { get; }
        public ItemLotGroup[] Churches { get; }
        public ItemLotGroup[] MajorBosses { get; }
        public ItemLotGroup[] MinorBosses { get; }
        public int[] SorceryIds { get; }
        public int[] AshOfWarIds { get; }
        public int[] IncantationIds { get; }
        public ItemLotGroup[] TeardropScarabs { get; }
        public ItemLotGroup[] Carriages { get; }
        public ItemLotGroup[] GoldenTrees { get; }
        public ItemLotGroup[] MapPlinths { get; }
        public int[] GoodRuneIds { get; }
        public int[] ShopLineupIds { get; }

        public GameData(RegulationParams regulationParams)
        {
            RegulationParams = regulationParams;

            ShopLineupIds = new int[]
            {
                100000, //  [Gatekeeper Gostoc] Festering Bloody Finger
                100002, //  [Gatekeeper Gostoc] Ruin Fragment
                100004, //  [Gatekeeper Gostoc] Buckler
                100009, //  [Gatekeeper Gostoc] Furlcalling Finger Remedy
                100010, //  [Gatekeeper Gostoc] Silver-Pickled Fowl Foot
                100011, //  [Gatekeeper Gostoc] Stormhawk Feather
                100012, //  [Gatekeeper Gostoc] Caestus
                100013, //  [Gatekeeper Gostoc] Great Arrow
                100014, //  [Gatekeeper Gostoc] Ballista Bolt
                100015, //  [Gatekeeper Gostoc] Bandit Manchettes
                100016, //  [Gatekeeper Gostoc] Bandit Boots
                100017, //  [Gatekeeper Gostoc] Bandit Garb
                100018, //  [Gatekeeper Gostoc] Ancient Dragon Smithing Stone
                100050, //  [Sorceress Sellen] Glintstone Pebble
                100051, //  [Sorceress Sellen] Scholar's Armament
                100052, //  [Sorceress Sellen] Scholar's Shield
                100053, //  [Sorceress Sellen] Crystal Barrage
                100054, //  [Sorceress Sellen] Glintstone Arc
                100055, //  [Sorceress Sellen] Glintstone Stars
                100061, //  [Sorceress Sellen] Glintblade Phalanx
                100062, //  [Sorceress Sellen] Carian Slicer
                100056, //  [Sorceress Sellen - Quest] Shard Spiral
                100057, //  [Sorceress Sellen - Conspectus Scroll] Glintstone Cometshard
                100058, //  [Sorceress Sellen - Conspectus Scroll] Star Shower
                100059, //  [Sorceress Sellen - Academy Scroll] Great Glintstone Shard
                100060, //  [Sorceress Sellen - Academy Scroll] Swift Glintstone Shard
                100500, //  [Merchant Kale] Note: Flask of Wondrous Physick
                100501, //  [Merchant Kale] Crafting Kit
                100502, //  [Merchant Kale] Nomadic Warrior's Cookbook [1]
                100503, //  [Merchant Kale] Nomadic Warrior's Cookbook [2]
                100504, //  [Merchant Kale] Missionary's Cookbook [1]
                100505, //  [Merchant Kale] Furlcalling Finger Remedy
                100506, //  [Merchant Kale] Cracked Pot
                100507, //  [Merchant Kale] Throwing Dagger
                100510, //  [Merchant Kale] Torch
                100511, //  [Merchant Kale] Large Leather Shield
                100513, //  [Merchant Kale] Telescope
                100514, //  [Merchant Kale] Note: Waypoint Ruins
                100515, //  [Merchant Kale] Chain Coif
                100516, //  [Merchant Kale] Chain Armor
                100517, //  [Merchant Kale] Chain Gauntlets
                100518, //  [Merchant Kale] Chain Leggings
                100519, //  [Merchant Kale] Arrow
                100520, //  [Merchant Kale] Bolt
                100075, //  [Knight Bernahl] Ash of War: Impaling Thrust
                100076, //  [Knight Bernahl] Ash of War: Spinning Slash
                100077, //  [Knight Bernahl] Ash of War: Quickstep
                100078, //  [Knight Bernahl] Ash of War: Stamp (Upward Cut)
                100079, //  [Knight Bernahl] Ash of War: Kick
                100080, //  [Knight Bernahl] Ash of War: War Cry
                100081, //  [Knight Bernahl] Ash of War: Endure
                100082, //  [Knight Bernahl] Ash of War: Parry
                100083, //  [Knight Bernahl] Ash of War: Storm Blade
                100084, //  [Knight Bernahl] Ash of War: Eruption
                100085, //  [Knight Bernahl] Ash of War: Assassin's Gambit
                100086, //  [Knight Bernahl] Ash of War: No Skill
                100100, //  [Patches] Margit's Shackle
                100101, //  [Patches] Missionary's Cookbook [2]
                100102, //  [Patches] Stonesword Key
                100103, //  [Patches] Furlcalling Finger Remedy
                100104, //  [Patches] Glass Shard
                100105, //  [Patches] Gold-Pickled Fowl Foot
                100108, //  [Patches] Parrying Dagger
                100111, //  [Patches] Sacrificial Twig
                100112, //  [Patches] Festering Bloody Finger
                100113, //  [Patches] Grace Mimic
                100114, //  [Patches] Fan Daggers
                100115, //  [Patches] Great Arrow
                100116, //  [Patches] Ballista Bolt
                100117, //  [Patches] Festering Bloody Finger
                100118, //  [Patches] Fan Daggers
                100119, //  [Patches] Estoc
                100120, //  [Patches] Great Arrow
                100121, //  [Patches] Ballista Bolt
                100122, //  [Patches] Horse Crest Wooden Shield
                100126, //  [D, Hunter of The Dead] Litany of Proper Death
                100127, //  [D, Hunter of The Dead] Order's Blade
                100150, //  [Blackguard Big Boggart] Rya's Necklace
                100155, //  [Blackguard Big Boggart] Boiled Crab
                100160, //  [Blackguard Big Boggart] Boiled Prawn
                100175, //  [Gowry] Night Shard
                100176, //  [Gowry] Night Maiden's Mist
                100177, //  [Gowry] Glintstone Stars
                100185, //  [Gowry] Pest Threads
                100200, //  [Sorcerer Rogier] Ash of War: Spinning Weapon
                100201, //  [Sorcerer Rogier] Ash of War: Glintstone Pebble
                100202, //  [Sorcerer Rogier] Ash of War: Carian Greatsword
                100225, //  [Sorcerer Rogier] Somber Smithing Stone [1]
                100226, //  [Sorcerer Rogier] Somber Smithing Stone [2]
                100227, //  [Sorcerer Rogier] Somber Smithing Stone [3]
                100228, //  [Sorcerer Rogier] Somber Smithing Stone [4]
                100229, //  [Sorcerer Rogier] Carian Filigreed Crest
                100250, //  [Preceptor Seluvis] Glintstone Pebble
                100251, //  [Preceptor Seluvis] Glintstone Arc
                100252, //  [Preceptor Seluvis] Starlight
                100275, //  [Preceptor Seluvis] Carian Phalanx
                100276, //  [Preceptor Seluvis] Glintstone Icecrag
                100277, //  [Preceptor Seluvis] Freezing Mist
                100278, //  [Preceptor Seluvis] Carian Retaliation
                100279, //  [Preceptor Seluvis] Glintstone Cometshard
                100280, //  [Preceptor Seluvis] Star Shower
                100281, //  [Preceptor Seluvis] Great Glintstone Shard
                100282, //  [Preceptor Seluvis] Swift Glintstone Shard
                100283, //  [Preceptor Seluvis] Glintblade Phalanx
                100284, //  [Preceptor Seluvis] Carian Slicer
                100300, //  [Preceptor Seluvis - Ranni Quest] Finger Maiden Therolina Puppet
                100301, //  [Preceptor Seluvis - Ranni Quest] Dolores the Sleeping Arrow Puppet
                100302, //  [Preceptor Seluvis - Ranni Quest] Jarwight Puppet
                100310, //  [Preceptor Seluvis - Dung Eater Quest] Dung Eater Puppet
                100325, //  [Pidia, Carian Servant] Glintstone Craftsman's Cookbook [7]
                100326, //  [Pidia, Carian Servant] Ritual Pot
                100329, //  [Pidia, Carian Servant] Old Fang
                100330, //  [Pidia, Carian Servant] Slumbering Egg
                100334, //  [Pidia, Carian Servant] Ash of War: Carian Retaliation
                100335, //  [Pidia, Carian Servant] Celestial Dew
                100336, //  [Pidia, Carian Servant] Larval Tear
                100337, //  [Pidia, Carian Servant] Budding Horn
                100338, //  [Pidia, Carian Servant] Ripple Blade
                100339, //  [Pidia, Carian Servant] Black Leather Shield
                100350, //  [Brother Corhyn] Rejection
                100351, //  [Brother Corhyn] Urgent Heal
                100352, //  [Brother Corhyn] Heal
                100353, //  [Brother Corhyn] Cure Poison
                100354, //  [Brother Corhyn] Flame Fortification
                100355, //  [Brother Corhyn] Magic Fortification
                100356, //  [Brother Corhyn] Catch Flame
                100357, //  [Brother Corhyn] Flame Sling
                100358, //  [Brother Corhyn - Altus Plateau] Great Heal
                100359, //  [Brother Corhyn - Altus Plateau] Lightning Fortification
                100360, //  [Brother Corhyn - Goldmask] Discus of Light
                100361, //  [Brother Corhyn - Erdtree Sanctuary] Immutable Shield
                100362, //  [Brother Corhyn - Fire Monks' Prayerbook] O, Flame!
                100363, //  [Brother Corhyn - Fire Monks' Prayerbook] Surge
                100364, //  [Brother Corhyn - Giant's Prayerbook] Giantsflame Take Thee
                100365, //  [Brother Corhyn - Giant's Prayerbook] Flame
                100366, //  [Brother Corhyn - Godskin Prayerbook] Black Flame
                100367, //  [Brother Corhyn - Godskin Prayerbook] Black Flame Blade
                100368, //  [Brother Corhyn - Two Fingers' Prayerbook] Lord's Heal
                100369, //  [Brother Corhyn - Two Fingers' Prayerbook] Lord's Aid
                100370, //  [Brother Corhyn - Assassin's Prayerbook] Assassin's Approach
                100371, //  [Brother Corhyn - Assassin's Prayerbook] Darkness
                100372, //  [Brother Corhyn - Golden Order Principia] Radagon's Rings of Light
                100373, //  [Brother Corhyn - Golden Order Principia] Law of Regression
                100374, //  [Brother Corhyn - Dragon Cult Prayerbook] Lightning Spear
                100375, //  [Brother Corhyn - Dragon Cult Prayerbook] Honed Bolt
                100376, //  [Brother Corhyn - Dragon Cult Prayerbook] Electrify Armament
                100377, //  [Brother Corhyn - Ancient Dragon Prayerbook] Ancient Dragons' Lightning Spear
                100378, //  [Brother Corhyn - Ancient Dragon Prayerbook] Ancient Dragons' Lightning Strike
                100400, //  [Miriel] Magic Glintblade
                100401, //  [Miriel] Carian Greatsword
                100406, //  [Miriel] Glintblade Phalanx
                100407, //  [Miriel] Carian Slicer
                100425, //  [Miriel] Blessing's Boon
                100402, //  [Miriel - Conspectus Scroll] Glintstone Cometshard
                100403, //  [Miriel - Conspectus Scroll] Star Shower
                100404, //  [Miriel - Academy Scroll] Great Glintstone Shard
                100405, //  [Miriel - Academy Scroll] Swift Glintstone Shard
                100426, //  [Miriel - Fire Monks' Prayerbook] O, Flame!
                100428, //  [Miriel - Fire Monks' Prayerbook] Surge
                100429, //  [Miriel - Giant's Prayerbook] Giantsflame Take Thee
                100430, //  [Miriel - Giant's Prayerbook] Flame
                100431, //  [Miriel - Godskin Prayerbook] Black Flame
                100432, //  [Miriel - Godskin Prayerbook] Black Flame Blade
                100433, //  [Miriel - Two Fingers' Prayerbook] Lord's Heal
                100434, //  [Miriel - Two Fingers' Prayerbook] Lord's Aid
                100435, //  [Miriel - Assassin's Prayerbook] Assassin's Approach
                100436, //  [Miriel - Assassin's Prayerbook] Darkness
                100437, //  [Miriel - Golden Order Principia] Radagon's Rings of Light
                100438, //  [Miriel - Golden Order Principia] Law of Regression
                100439, //  [Miriel - Dragon Cult Prayerbook] Lightning Spear
                100440, //  [Miriel - Dragon Cult Prayerbook] Honed Bolt
                100441, //  [Miriel - Dragon Cult Prayerbook] Electrify Armament
                100442, //  [Miriel - Ancient Dragon Prayerbook] Ancient Dragons' Lightning Spear
                100443, //  [Miriel - Ancient Dragon Prayerbook] Ancient Dragons' Lightning Strike
                101800, //  [Twin Maiden Husks] White Cipher Ring
                101801, //  [Twin Maiden Husks] Blue Cipher Ring
                101802, //  [Twin Maiden Husks] Spirit Calling Bell
                101803, //  [Twin Maiden Husks] Lone Wolf Ashes
                101804, //  [Twin Maiden Husks - Haligtree] Black Flame's Protection
                101805, //  [Twin Maiden Husks - Haligtree] Lord's Divine Fortification
                101806, //  [Twin Maiden Husks - Mohgwyn] Fevor's Cookbook [3]
                101807, //  [Twin Maiden Husks - Mohg] Law of Causality
                101859, //  [Twin Maiden Husks] Memory Stone
                101860, //  [Twin Maiden Husks] Stonesword Key
                101861, //  [Twin Maiden Husks] Rune Arc
                101862, //  [Twin Maiden Husks] Furled Finger's Trick-Mirror
                101863, //  [Twin Maiden Husks] Host's Trick-Mirror
                101864, //  [Twin Maiden Husks] Dagger
                101865, //  [Twin Maiden Husks] Longsword
                101866, //  [Twin Maiden Husks] Scimitar
                101867, //  [Twin Maiden Husks] Battle Axe
                101868, //  [Twin Maiden Husks] Mace
                101869, //  [Twin Maiden Husks] Short Spear
                101870, //  [Twin Maiden Husks] Rapier
                101871, //  [Twin Maiden Husks] Finger Seal
                101872, //  [Twin Maiden Husks] Longbow
                101873, //  [Twin Maiden Husks] Heater Shield
                101874, //  [Twin Maiden Husks] Knight Helm
                101875, //  [Twin Maiden Husks] Knight Armor
                101876, //  [Twin Maiden Husks] Knight Gauntlets
                101877, //  [Twin Maiden Husks] Knight Greaves
                101878, //  [Twin Maiden Husks] Flask of Wondrous Physick
                101879, //  [Twin Maiden Husks] Crafting Kit
                101880, //  [Twin Maiden Husks] Whetstone Knife
                101881, //  [Twin Maiden Husks] Talisman Pouch
            };

            GoodRuneIds = new int[]
            {
                2912,
                2913,
                2914,
                2915,
                2916,
                2917,
                2918,
                2919
            };

            MapPlinths = new ItemLotGroup[]
            {
                new ItemLotGroup(12010000), // [LD - Ainsel/Lake of Rot] Map: Ainsel
                new ItemLotGroup(12010010), // [LD - Ainsel/Lake of Rot] Map: Lake of Rot
                new ItemLotGroup(12020060), // [LD - Nokron / Siofra] Map: Siofra River
                new ItemLotGroup(12030000), // [LD - Deeproot Depths] Map: Deeproot Depths
                new ItemLotGroup(12050000), // [LD - Mohgwyn] Map: Mohgwyn Palace
                new ItemLotGroup(1034480200), // [Liurnia of the Lakes - Kingsrealm Ruins] Map: Liurnia, West
                new ItemLotGroup(1036540500), // [Mt. Gelmir - Volcano Manor Entrance] Map: Mt. Gelmir
                new ItemLotGroup(1037440210), // [Liurnia of the Lakes - Gate Town Southeast] Map: Liurnia, North
                new ItemLotGroup(1038410200), // [Liurnia of the Lakes - Laskyar Ruins] Map: Liurnia, East
                new ItemLotGroup(1040520500), // [Altus Plateau - Forest-Spanning Greatbridge] Map: Altus Plateau
                new ItemLotGroup(1042370200), // [Limgrave - Gatefront] Map: Limgrave, West
                new ItemLotGroup(1042510500), // [Leyndell - Outer Wall Phantom Tree] Map: Leyndell, Royal Capital
                new ItemLotGroup(1044320000), // [Weeping Peninsula - Castle Morne Approach] Map: Weeping Peninsula
                new ItemLotGroup(1045370020), // [Limgrave - Mistwood] Map: Limgrave, East
                new ItemLotGroup(1048560700), // [Consecrated Snowfield - South of Ordina] Map: Consecrated Snowfield
                new ItemLotGroup(1049370500), // [Caelid - Southern Aeonia Swamp Bank] Map: Caelid
                new ItemLotGroup(1049400500), // [Greyoll's Dragonbarrow - Deep Siofra Well] Map: Dragonbarrow
                new ItemLotGroup(1049530700), // [Mountaintops of the Giants - West Zamor Ruins] Map: Mountaintops of the Giants, West
                new ItemLotGroup(1052540700), // [Mountaintops of the Giants - Northeast Giants' Gravepost] Map: Mountaintops of the Giants, East
            };

            GoldenTrees = new ItemLotGroup[]
            {
                new ItemLotGroup(10000730), //  [LD - Stormveil] Golden Seed
                new ItemLotGroup(10001095), //  [LD - Stormveil] Golden Seed
                new ItemLotGroup(11000990), //  [LD - Leyndell] Golden Seed
                new ItemLotGroup(11001193), //  [LD - Leyndell] Golden Seed
                new ItemLotGroup(12010100), //  [LD - Ainsel/Lake of Rot] Golden Seed
                new ItemLotGroup(12015997), //  [LD - Ainsel/Lake of Rot] Golden Seed
                new ItemLotGroup(12020040), //  [LD - Nokron / Siofra] Golden Seed
                new ItemLotGroup(12050010), //  [LD - Mohgwyn] Golden Seed
                new ItemLotGroup(13000980), //  [LD - Crumbling Farum Azula] Golden Seed
                new ItemLotGroup(13000990), //  [LD - Crumbling Farum Azula] Golden Seed
                new ItemLotGroup(14000990), //  [LD - Academy of Raya Lucaria] Golden Seed
                new ItemLotGroup(15001200), //  [LD - Elphael / Miquella's Haligtree] Golden Seed
                new ItemLotGroup(1035460100), //  [Liurnia of the Lakes - Main Academy Gate] Golden Seed
                new ItemLotGroup(1035500300), //  [Liurnia of the Lakes - Caria Manor] Golden Seed
                new ItemLotGroup(1036440300), //  [Liurnia of the Lakes - Gate Town Southwest] Golden Seed
                new ItemLotGroup(1036540400), //  [Mt. Gelmir - Volcano Manor Entrance] Golden Seed
                new ItemLotGroup(1037500100), //  [Liurnia of the Lake - Northeast Ravine] Golden Seed
                new ItemLotGroup(1037530400), //  [Mt. Gelmir - Primeval Sorcerer Azur] Golden Seed
                new ItemLotGroup(1038510400), //  [Altus Plateau - Lux Ruins] Golden Seed
                new ItemLotGroup(1039510400), //  [Altus Plateau - Altus Highway Junction] Golden Seed
                new ItemLotGroup(1041380100), //  [Stormhill - Stormhill Shack] Golden Seed
                new ItemLotGroup(1041540400), //  [Altus Plateau - West Windmill Village] Golden Seed
                new ItemLotGroup(1042500020), //  [Leyndell - South of Outer Wall Phantom Tree] Golden Seed
                new ItemLotGroup(1042510400), //  [Leyndell - Outer Wall Phantom Tree] Golden Seed
                new ItemLotGroup(1042510410), //  [Leyndell - Outer Wall Phantom Tree] Golden Seed
                new ItemLotGroup(1042540400), //  [Altus Plateau - Highway Lookout Tower] Golden Seed
                new ItemLotGroup(1043520400), //  [Leyndell - Southeast Outer Wall Battleground] Golden Seed
                new ItemLotGroup(1043520410), //  [Leyndell - Southeast Outer Wall Battleground] Golden Seed
                new ItemLotGroup(1044320020), //  [Weeping Peninsula - Castle Morne Approach] Golden Seed
                new ItemLotGroup(1046360100), //  [Limgrave - Fort Haight] Golden Seed
                new ItemLotGroup(1048570800), //  [Consecrated Snowfield - Ordina, Liturgical Town] Golden Seed
                new ItemLotGroup(1049370020), //  [Caelid - Southern Aeonia Swamp Bank] Golden Seed
                new ItemLotGroup(1049520800), //  [Mountaintops of the Giants - Before Grand Lift of Rold] Golden Seed
                new ItemLotGroup(1049550800), //  [Consecrated Snowfield - Northeast Foggy Area] Golden Seed
                new ItemLotGroup(1050390100), //  [Caelid - East Sellia] Golden Seed
                new ItemLotGroup(1051430020), //  [Greyoll's Dragonbarrow - Bestial Sanctum] Golden Seed
                new ItemLotGroup(1052530800), //  [Mountaintops of the Giants - Northwest Fire Giant Arena] Golden Seed
                new ItemLotGroup(1052570800), //  [Mountaintops of the Giants - Before Freezing Lake] Golden Seed
            };

            Carriages = new ItemLotGroup[]
            {
                new ItemLotGroup(1033470200), // [Liurnia of the Lakes - The Four Belfries] Carian Knight's Sword
                new ItemLotGroup(1038510090), // [Altus Plateau - Lux Ruins] Troll's Golden Sword
                new ItemLotGroup(1042500000), // [Leyndell - South of Outer Wall Phantom Tree] Giant-Crusher
                new ItemLotGroup(1044330210), // [Weeping Peninsula - Ailing Village Outskirts] Morning Star
                new ItemLotGroup(1040540000), // [Altus Plateau - Road of Iniquity Side Path] Great Stars
                new ItemLotGroup(1049540900), // [Consecrated Snowfield - Hidden Path to the Haligtree] St. Trina's Torch
                new ItemLotGroup(1040400000), // [Liurnia - Liurnia Highway South Endpoint] Treespear
                new ItemLotGroup(1047400920), // [Caelid - Caelem Ruins] Greatsword
                new ItemLotGroup(1048550900), // [Consecrated Snowfield - Northwest Foggy Area] Flowing Curved Sword
                new ItemLotGroup(944360200), // [Troll Carriage - Waypoint Ruins] Greataxe
                new ItemLotGroup(942370070), // [Troll Carriage - Gatefront Ruins] Lordsworn's Greatsword
                new ItemLotGroup(942370060), // [Troll Carriage - Gatefront Ruins] Flail
            };

            var spirits = new List<int>();
            for (int i = 0; i <= 63; i++)
            {
                spirits.Add(200000 + i * 1000 + 10);
            }
            SpiritAshIds = spirits.ToArray();

            AshOfWarIds = RegulationParams.EquipParamGem.Where(row => row.Id >= 10000).Select(row => row.Id).ToArray();

            Talismans = new int[]
        {
            1002,
            1012,
            1022,
            1031,
            1032,
            1042,
            1050,
            1051,
            1060,
            1070,
            1080,
            1090,
            1100,
            1140,
            1150,
            1161,
            1171,
            1181,
            1190,
            1191,
            1201,
            1210,
            1220,
            1221,
            1230,
            1231,
            1250,
            2000,
            2010,
            2020,
            2030,
            2040,
            2050,
            2060,
            2070,
            2080,
            2081,
            2090,
            2100,
            2110,
            2120,
            2130,
            2140,
            2150,
            2160,
            2170,
            2180,
            2190,
            2200,
            2210,
            2220,
            3000,
            3001,
            3040,
            3050,
            3060,
            3070,
            3080,
            3090,
            4002,
            4003,
            4012,
            4022,
            4032,
            4042,
            4052,
            4060,
            4070,
            4080,
            4090,
            4100,
            4110,
            5000,
            5010,
            5020,
            5030,
            5040,
            5050,
            5060,
            6000,
            6010,
            6020,
            6040,
            6050,
            6060,
            6070,
            6080,
            6090,
            6100,
            6110
        };
            Tears = new int[]
            {
                11000,
                11001,
                11002,
                11003,
                11004,
                11005,
                11006,
                11007,
                11008,
                11009,
                11010,
                11011,
                11012,
                11013,
                11014,
                11015,
                11016,
                11017,
                11018,
                11019,
                11020,
                11021,
                11022,
                11023,
                11024,
                11025,
                11026,
                11027,
                11028,
                11029,
                11030,
                11031
            };

            Churches = new ItemLotGroup[] {
                new ItemLotGroup(1037460000, 1037460001), // Liurnia - Church of vows
                new ItemLotGroup(1037490100, 1037490031, 1037490032, 1037490030), // Liurnia - Church of inhibition 
                new ItemLotGroup(1035440200), // Liurnia - Rose church
                new ItemLotGroup(1039390000), // Liurnia - Church of Irith
                new ItemLotGroup(1039520400), // Altus - Second church of marika
                new ItemLotGroup(1040510400), // Altus - Stormcaller church
                new ItemLotGroup(1041330200), // Weeping Peninsula - Fourth church of marika
                new ItemLotGroup(1041350020), // Limgrave - Church of dragon communion
                new ItemLotGroup(1042360050, 1042360060), // Limgrave - Church of elleh
                new ItemLotGroup(1043500000, 1043500010, 1043500030), // Leyndell - Minor Erdtree Church 
                new ItemLotGroup(1046380100, 1046380300, 1046380301), // Limgrave - Third Church of Marika
                new ItemLotGroup(1046400030, 1046400050), // Caelid - Smoldering Church
                new ItemLotGroup(1054550310, 1054550800), // Mountaintops of the Giants - First Church of Marika
                new ItemLotGroup(1036490000), // Liurnia - Church of bellum
                new ItemLotGroup(1043350100), // Weeping Peninsula - Church of pilgrimage
                new ItemLotGroup(1044330100), // Weeping Peninsula - Callu baptismal church
                new ItemLotGroup(1050380020), // Caelid - Church of the plague
                new ItemLotGroup(1051530800), // Mountaintops of the Giants - Church of Repose
            };

            MajorBosses = new ItemLotGroup[] {
                new ItemLotGroup(10000), //Stormveil - Margit] Talisman Pouch
                new ItemLotGroup(10010), //Stormveil - Godrick] Godrick's Great Rune
                new ItemLotGroup(10011), //Stormveil - Godrick] Remembrance of the Grafted
                new ItemLotGroup(10040), //Leyndell - Morgott] Remembrance of the Omen King
                new ItemLotGroup(10041), //Leyndell - Morgott] Morgott's Great Rune
                new ItemLotGroup(10060), //Ashen Leyndell - Gideon] Scepter of the All-Knowing
                new ItemLotGroup(10061), //Ashen Leyndell - Gideon] All-Knowing Helm
                new ItemLotGroup(10062), //Ashen Leyndell - Gideon] All-Knowing Armor
                new ItemLotGroup(10063), //Ashen Leyndell - Gideon] All-Knowing Gauntlets
                new ItemLotGroup(10064), //Ashen Leyndell - Gideon] All-Knowing Greaves
                new ItemLotGroup(10070), //Ashen Leyndell - Hoarah Loux] Remembrance of Hoarah Loux
                new ItemLotGroup(10120), // Mohgwyn Palace - Mohg] Remembrance of the Blood Lord
                new ItemLotGroup(10121), // Mohgwyn Palace - Mohg] Mohg's Great Rune
                new ItemLotGroup(10140), // Farum Azula - Godskin Duo] Smithing-Stone Miner's Bell Bearing), // 4]
                new ItemLotGroup(10141), // Farum Azula - Godskin Duo] Ash of War: Black Flame Tornado
                new ItemLotGroup(10170), // Raya Lucaria - Red Wolf of Radagon] Memory Stone
                new ItemLotGroup(10180), // Raya Lucaria - Rennala] Remembrance of the Full Moon Queen
                //10181, [Raya Lucaria - Rennala] Great Rune of the Unborn
                new ItemLotGroup(10182), // Raya Lucaria - Rennala] None
                new ItemLotGroup(10190), // Haligtree - Loretta] Loretta's Mastery
                new ItemLotGroup(10191), // Haligtree - Loretta] Loretta's War Sickle
                new ItemLotGroup(10200), // Haligtree - Malenia] Remembrance of the Rot Goddess
                new ItemLotGroup(10201), // Haligtree - Malenia] Malenia's Great Rune
                new ItemLotGroup(10210), // Volcano Manor - Godskin Noble] Godskin Stitcher
                new ItemLotGroup(10211), // Volcano Manor - Godskin Noble] Noble Presence
                new ItemLotGroup(10160), // Farum Azula - Maliketh] Remembrance of the Black Blade
                new ItemLotGroup(10220), // Mt. Gelmir - Rykard] Remembrance of the Blasphemous
                new ItemLotGroup(10221), // Mt. Gelmir - Rykard] Rykard's Great Rune
                new ItemLotGroup(10230), // Erdtree - Elden Beast] Elden Remembrance
                new ItemLotGroup(10250), // Mohgwyn Palace - Mohg] Bloodflame Talons
                new ItemLotGroup(10300), // Caelid - Radahn] Remembrance of the Starscourge
                new ItemLotGroup(10301), // Caelid - Radahn] Radahn's Great Rune
                new ItemLotGroup(10310), // Mountaintops - Fire Giant] Remembrance of the Fire Giant
            };

            MinorBosses = new ItemLotGroup[] {
                new ItemLotGroup(10030), //Chapel of Anticipation - Grafed Scion] Ornamental Straight Sword
                new ItemLotGroup(10031), //Chapel of Anticipation - Grafed Scion] Golden Beast Crest Shield
                new ItemLotGroup(10080), // Lake or Rot - Astel] Remembrance of the Naturalborn
                new ItemLotGroup(10090), // Ainsel - Dragonkin Soldier] Frozen Lightning Spear
                new ItemLotGroup(10100), // Siofra - Valiant Gargoyle] Gargoyle's Greatsword
                new ItemLotGroup(10101), // Siofra - Valiant Gargoyle] Gargoyle's Twinblade
                new ItemLotGroup(10110), // Deeproot Depths - Fortissax] Remembrance of the Lichdragon
                new ItemLotGroup(10150), // Farum Azula - Dragonlord Placidusax] Remembrance of the Dragonlord
                new ItemLotGroup(10260), // Ruin-Strewn Precipice - Magma Wyrm Makar] Magma Wyrm's Scalesword
                new ItemLotGroup(10261), // Ruin-Strewn Precipice - Magma Wyrm Makar] Dragon Heart
                new ItemLotGroup(10280), // Fringefolk Hero's Grave - Ulcerated Tree Spirit] Golden Seed
                new ItemLotGroup(10281), // Fringefolk Hero's Grave - Ulcerated Tree Spirit] Banished Knight Oleg
                new ItemLotGroup(10290), // Volcano Manor - Abducator Virgins] Inquisitor's Girandole
                new ItemLotGroup(10320), // Siofra - Ancestor Spirit] Ancestral Follower Ashes
                new ItemLotGroup(10330), // Nokron - Regal Ancestor Spirit] Remembrance of the Regal Ancestor
                new ItemLotGroup(10340), // Nokron - Mimic Tear] Larval Tear
                new ItemLotGroup(10341), // Nokron - Mimic Tear] Silver Tear Mask
                new ItemLotGroup(10350), // Deeproot Depths - Fia's Champions] Fia's Mist
                new ItemLotGroup(10730), // Mountaintops - Sanguine Noble] Sanguine Noble Hood
                new ItemLotGroup(10731), // Mountaintops - Sanguine Noble] Sanguine Noble Robe
                new ItemLotGroup(10732), // Mountaintops - Sanguine Noble] Sanguine Noble Bracelets
                new ItemLotGroup(10733), // Mountaintops - Sanguine Noble] Sanguine Noble Trousers
                new ItemLotGroup(10740), // Capital Outskirts - Fell Twins] Omenkiller Rollo
                new ItemLotGroup(10800), // Weeping Penisula - Leonine Misbegotten] Blade Greatsword
                new ItemLotGroup(10810), // Caria Manor - Loretta] Loretta's Greatbow
                new ItemLotGroup(10811), // Caria Manor - Loretta] Ash of War: Loretta's Slash
                new ItemLotGroup(10820), // Shaded Castle - Elemer of the Briar] Marais Executioner's Sword
                new ItemLotGroup(10821), // Shaded Castle - Elemer of the Briar] Briar Greatshield
                new ItemLotGroup(10830), // Redmane Castle - Misbegotten Warrior/Crucible Knight] Ruins Greatsword
                new ItemLotGroup(10840), // Castle Sol - Commander Niall] Veteran's Prosthesis
                new ItemLotGroup(20000), // Tombsward Catacombs - Cemetery Shade] Lhutel the Headless
                new ItemLotGroup(20010), // Impaler's Catacombs - Erdtree Burial Watchdog] Demi-Human Ashes
                new ItemLotGroup(20020), // Stormfoot Catacombs - Erdtree Burial Watchdog] Noble Sorcerer Ashes
                new ItemLotGroup(20030), // Deathtouched Catacombs - Black Knife Assassin] Assassin's Crimson Dagger
                new ItemLotGroup(20040), // Murkwater Catacombs - Grave Warden Duelist] Battle Hammer
                new ItemLotGroup(20050), // Black Knife Catacombs - Cemetery Shade] Twinsage Sorcerer Ashes
                new ItemLotGroup(20060), // Road's End Catacombs - Spirit-caller Snail] Glintstone Sorcerer Ashes
                new ItemLotGroup(20070), // Cliffbottom Catacombs - Erdtree Burial Watchdog] Kaiden Sellsword Ashes
                new ItemLotGroup(20080), // Sainted Hero's Grave - Leyndell] Ancient Dragon Knight Kristoff
                new ItemLotGroup(20090), // Gelmir's Heo's Grave - Mt. Gelmir] Bloodhound Knight Floh
                new ItemLotGroup(20100), // Auriza Hero's Grave - Crucible Knigh Ordovis] Ordovis's Greatsword
                new ItemLotGroup(20101), // Auriza Hero's Grave - Crucible Knigh Ordovis] Crucible Axe Helm
                new ItemLotGroup(20102), // Auriza Hero's Grave - Crucible Knigh Ordovis] Crucible Axe Armor
                new ItemLotGroup(20103), // Auriza Hero's Grave - Crucible Knigh Ordovis] Crucible Gauntlets
                new ItemLotGroup(20104), // Auriza Hero's Grave - Crucible Knigh Ordovis] Crucible Greaves
                new ItemLotGroup(20110), // Unslightly Catacombs - Perfumer Tricia] Perfumer Tricia
                new ItemLotGroup(20120), // Wyndham Catacombs - Erdtree Burial Watchdog] Glovewort Picker's Bell Bearing), // 1]
                new ItemLotGroup(20130), // Auriza Side Tomb - Grave Warden Duelist] Soldjars of Fortune Ashes
                new ItemLotGroup(20140), // Minor Erdtree Catacombs - Erdtree Burial Watchdog] Mad Pumpkin Head Ashes
                new ItemLotGroup(20150), // Caelid Catacombs - Cemetery Shade] Kindred of Rot Ashes
                new ItemLotGroup(20160), // War-Dead Catacombs - Putrid Tree Spirit] Redmane Knight Ogha
                new ItemLotGroup(20161), // War-Dead Catacombs - Putrid Tree Spirit] Golden Seed
                new ItemLotGroup(20170), // Giant-Conquering Hero's Grave - Ancient Hero of Zamor] Zamor Curved Sword
                new ItemLotGroup(20171), // Giant-Conquering Hero's Grave - Ancient Hero of Zamor] Zamor Mask
                new ItemLotGroup(20172), // Giant-Conquering Hero's Grave - Ancient Hero of Zamor] Zamor Armor
                new ItemLotGroup(20173), // Giant-Conquering Hero's Grave - Ancient Hero of Zamor] Zamor Bracelets
                new ItemLotGroup(20174), // Giant-Conquering Hero's Grave - Ancient Hero of Zamor] Zamor Legwraps
                new ItemLotGroup(20180), // Giants' Mountaintop Catacombs - Ulcerated Tree Spirit] Golden Seed
                new ItemLotGroup(20181), // Giants' Mountaintop Catacombs - Ulcerated Tree Spirit] Glovewort Picker's Bell Bearing
                new ItemLotGroup(20190), // Consecrated Snowfiled Catacombs - Putrid Grave Warden Duelist] Great Grave Glovewort
                new ItemLotGroup(20191), // Consecrated Snowfiled Catacombs - Putrid Grave Warden Duelist] Rotten Gravekeeper Cloak
                new ItemLotGroup(20200), // Hidden Path ot the Haligtree - Stray Mimic Tear] Blackflame Monk Amon
                new ItemLotGroup(20210), // Black Knife Catacombs - Black Knife Assassin] Assassin's Cerulean Dagger
                new ItemLotGroup(20211), // Black Knife Catacombs - Black Knife Assassin] Black Knifeprint
                new ItemLotGroup(20220), // Leyndell Catacombs - Esgar, Priest of Blood] Lord of Blood's Exultation
                new ItemLotGroup(20300), // Tombsward Cave - Miranda the Blighted Bloom] Viridian Amber Medallion
                new ItemLotGroup(20310), // Earthbore Cave - Runebear] Spelldrake Talisman
                new ItemLotGroup(20330), // Groveside Cave - Beastman of Farum Azula] Flamedrake Talisman
                new ItemLotGroup(20340), // Coastal Cave - Demi-Human Chief] Sewing Needle
                new ItemLotGroup(20341), // Coastal Cave - Demi-Human Chief] Tailoring Tools
                new ItemLotGroup(20350), // Highroad Cave - Guardian Golem] Blue Dancer Charm
                new ItemLotGroup(20360), // Stillwater Cave - Cleanrot Knight] Winged Sword Insignia
                new ItemLotGroup(20370), // Lakeside Crystal Cave - Bloodhound Knight] Cerulean Amber Medallion
                new ItemLotGroup(20380), // Academy Crystal Cave - Crystalians] Crystal Release
                new ItemLotGroup(20390), // Seethewater Cave - Kindred of Rot] Kindred of Rot's Exultation
                new ItemLotGroup(20400), // Volcano Cave - Demi-human Queen Margot] Jar Cannon
                new ItemLotGroup(20410), // Omenkiller - Perfumer's Grotto] Great Omenkiller Cleaver
                new ItemLotGroup(20420), // Sage's Cave - Black Knife Assassin] Concealing Veil
                new ItemLotGroup(20430), // Goal Cave - Frenzied Duelist] Putrid Corpse Ashes
                new ItemLotGroup(20440), // Dragonbarrow Cave - Beastman of Farum Azula] Flamedrake Talisman +2
                new ItemLotGroup(20450), // Abandoned Cave - Cleanrot Knight Duo] Gold Scarab
                new ItemLotGroup(20460), // Sellia Hideaway - Putrid Crystalians] Crystal Torrent
                new ItemLotGroup(20470), // Cave of the Forlorn - Misbegotten Crusader] Golden Order Greatsword
                new ItemLotGroup(20480), // Spiritcaller's Cave - Godskin Apostle/Noble] Godskin Swaddling Cloth
                new ItemLotGroup(20481), // Spiritcaller's Cave - Godskin Apostle/Noble] Black Flame Ritual
                new ItemLotGroup(20490), // Sage's Cave - Necromancer Garris] Family Heads
                new ItemLotGroup(20600), // Morne Tunnel - Scaly Misbegotten] Rusted Anchor
                new ItemLotGroup(20610), // Limgrave Tunnels - Stonedigger Troll] Roar Medallion
                new ItemLotGroup(20620), // Raya Lucaria Crystal Tunnel - Crystalian] Smithing-Stone Miner's Bell Bearing [1]
                new ItemLotGroup(20630), // Old Altus Tunnel - Stonedigger Troll] Great Club
                new ItemLotGroup(20640), // Sealed Tunnel - Onyx Lord] Onyx Lord's Greatsword
                new ItemLotGroup(20650), // Altus Tunnel - Crystalians] Somberstone Miner's Bell Bearing [2]
                new ItemLotGroup(20660), // Gael Tunnel - Magma Wyrm] Dragon Heart
                new ItemLotGroup(20661), // Gael Tunnel - Magma Wyrm] Moonveil
                new ItemLotGroup(20670), // Sellia Crystal Tunnel - Fallingstar Beast] Somber Smithing Stone), // 6]
                new ItemLotGroup(20671), // Sellia Crystal Tunnel - Fallingstar Beast] Smithing Stone), // 7]
                new ItemLotGroup(20672), // Sellia Crystal Tunnel - Fallingstar Beast] Gravity Stone Chunk
                new ItemLotGroup(20673), // Sellia Crystal Tunnel - Fallingstar Beast] Somberstone Miner's Bell Bearing), // 1]
                new ItemLotGroup(20680), // Yelough Anix Tunnel - Astel] Meteorite of Astel
                new ItemLotGroup(30100), // Limgrave - Field - Tree Sentinel] Golden Halberd
                new ItemLotGroup(30110), // Limgrave - Field - Flying Dragon Agheel] Dragon Heart
                new ItemLotGroup(30120), // Limgrave - Evergaol - Crucible Knight] Aspects of the Crucible: Tail
                new ItemLotGroup(30130), // Limgrave - Evergaol - Bloodhound Knight Darriwil] Bloodhound's Fang
                new ItemLotGroup(30170), // Limgrave - Field - Tibia Mariner] Deathroot
                new ItemLotGroup(30171), // Limgrave - Field - Tibia Mariner] Skeletal Militiaman Ashes
                new ItemLotGroup(30185), // Weeping Penisula - Field - Erdtree Avatar] Crimsonburst Crystal Tear
                new ItemLotGroup(30186), // Weeping Penisula - Field - Erdtree Avatar] Opaline Bubbletear
                new ItemLotGroup(30200), // Liurnia - Field - Erdtree Avatar] Magic-Shrouding Cracked Tear
                new ItemLotGroup(30201), // Liurnia - Field - Erdtree Avatar] Lightning-Shrouding Cracked Tear
                new ItemLotGroup(30202), // Liurnia - Field - Erdtree Avatar] Holy-Shrouding Cracked Tear
                new ItemLotGroup(30205), // Liurnia - Field - Erdtree Avatar] Cerulean Crystal Tear
                new ItemLotGroup(30206), // Liurnia - Field - Erdtree Avatar] Ruptured Crystal Tear
                new ItemLotGroup(30210), // Liurnia - Field - Glintstone Dragon Smarag] Dragon Heart
                new ItemLotGroup(30225), // Liurnia - Field - Omenkiller] Crucible Knot Talisman
                new ItemLotGroup(30240), // Liurnia - Field - Tibia Mariner] Deathroot
                new ItemLotGroup(30241), // Liurnia - Field - Tibia Mariner] Skeletal Bandit Ashes
                new ItemLotGroup(30245), // Liurnia - Evergaol - Adan, Thief of Fire] Flame of the Fell God
                new ItemLotGroup(30250), // Liurnia - Evergaol - Bols, Carian Knight] Greatblade Phalanx
                new ItemLotGroup(30255), // Liurnia - Evergaol - Onyx Lord] Meteorite
                new ItemLotGroup(30260), // Liurnia - Field - Glintstone Dragon Adula] Dragon Heart
                new ItemLotGroup(30261), // Liurnia - Field - Glintstone Dragon Adula] Adula's Moonblade
                new ItemLotGroup(30265), // Liurnia - Evergaol - Alecto, Black Knife Ringleader] Black Knife Tiche
                new ItemLotGroup(30300), // Altus Plateau - Field - Ancient Dragon Lansseax] Lansseax's Glaive
                new ItemLotGroup(30310), // Altus Plateau - Field - Fallingstar Beast] Somber Smithing Stone), // 5]
                new ItemLotGroup(30311), // Altus Plateau - Field - Fallingstar Beast] Smithing Stone), // 6]
                new ItemLotGroup(30312), // Altus Plateau - Field - Fallingstar Beast] Gravity Stone Fan
                new ItemLotGroup(30313), // Altus Plateau - Field - Fallingstar Beast] Gravity Stone Chunk
                new ItemLotGroup(30315), // Capital Outskirts - Field - Draconic Tree Sentinel] Dragon Greatclaw
                new ItemLotGroup(30316), // Capital Outskirts - Field - Draconic Tree Sentinel] Dragonclaw Shield
                new ItemLotGroup(30320), // Altus Plateau - Field - Wormface] Speckled Hardtear
                new ItemLotGroup(30321), // Altus Plateau - Field - Wormface] Crimsonspill Crystal Tear
                new ItemLotGroup(30325), // Altus Plateau - Field - Godskin Apostle] Godskin Peeler
                new ItemLotGroup(30326), // Altus Plateau - Field - Godskin Apostle] Scouring Black Flame
                new ItemLotGroup(30335), // Capital Outskirts - Field - Tree Sentinel Duo] Erdtree Greatshield
                new ItemLotGroup(30336), // Capital Outskirts - Field - Tree Sentinel Duo] Hero's Rune), // 1]
                new ItemLotGroup(30350), // Altus Plateau - Field - Black Knife Assassin] Black Knife
                new ItemLotGroup(30375), // Mt. Gelmir - Field - Full-grown Fallingstar Beast] Somber Smithing Stone), // 6]
                new ItemLotGroup(30376), // Mt. Gelmir - Field - Full-grown Fallingstar Beast] Smithing Stone), // 6]
                new ItemLotGroup(30377), // Mt. Gelmir - Field - Full-grown Fallingstar Beast] Fallingstar Beast Jaw
                new ItemLotGroup(30380), // Mt. Gelmir - Field - Ulcerated Tree Spirit] Leaden Hardtear
                new ItemLotGroup(30381), // Mt. Gelmir - Field - Ulcerated Tree Spirit] Cerulean Hidden Tear
                new ItemLotGroup(30385), // Altus Plateau - Field - Tibia Mariner] Deathroot
                new ItemLotGroup(30386), // Altus Plateau - Field - Tibia Mariner] Tibia's Summons
                new ItemLotGroup(30390), // Mt. Gelmir - Field - Magma Wyrm] Dragon Heart
                new ItemLotGroup(30395), // Mt. Gelmir - Field - Demi-Human Queen Maggie] Memory Stone
                new ItemLotGroup(30400), // Mt. Gelmir - Field - Magma Wyrm] Dragon Heart
                new ItemLotGroup(30405), // Caelid - Field - Commander O'Neil] Commander's Standard
                new ItemLotGroup(30406), // Caelid - Field - Commander O'Neil] Unalloyed Gold Needle
                new ItemLotGroup(30410), // Caelid - Field - Erdtree Avatar] Greenburst Crystal Tear
                new ItemLotGroup(30411), // Caelid - Field - Erdtree Avatar] Flame-Shrouding Cracked Tear
                new ItemLotGroup(30415), // Caelid - Field - Putrid Avatar] Opaline Hardtear
                new ItemLotGroup(30416), // Caelid - Field - Putrid Avatar] Stonebarb Cracked Tear
                new ItemLotGroup(30420), // Caelid - Field - Flying Dragon Greyll] Dragon Heart
                new ItemLotGroup(30425), // Caelid - Field - Blade Blade Kindred] Gargoyle's Blackblade
                new ItemLotGroup(30426), // Caelid - Field - Blade Blade Kindred] Gargoyle's Black Halberd
                new ItemLotGroup(30505), // Forbidden Lands - Field - Black Blade Kindred] Gargoyle's Black Blades
                new ItemLotGroup(30506), // Forbidden Lands - Field - Black Blade Kindred] Gargoyle's Black Axe
                new ItemLotGroup(30510), // Mountaintops of the Giants - Field - Borealis, the Freezing Fog] Dragon Heart
                new ItemLotGroup(30515), // Mountaintops of the Giants - Evergaol - Roundtable Knight Vyke] Vyke's Dragonbolt
                new ItemLotGroup(30516), // Mountaintops of the Giants - Evergaol - Roundtable Knight Vyke] Fingerprint Helm
                new ItemLotGroup(30517), // Mountaintops of the Giants - Evergaol - Roundtable Knight Vyke] Fingerprint Armor
                new ItemLotGroup(30518), // Mountaintops of the Giants - Evergaol - Roundtable Knight Vyke] Fingerprint Gauntlets
                new ItemLotGroup(30519), // Mountaintops of the Giants - Evergaol - Roundtable Knight Vyke] Fingerprint Greaves
                new ItemLotGroup(30525), // Mountaintops of the Giants - Field - Erdtree Avatar] Cerulean Crystal Tear
                new ItemLotGroup(30526), // Mountaintops of the Giants - Field - Erdtree Avatar] Crimson Bubbletear
                new ItemLotGroup(30530), // Mountaintops of the Giants - Field - Death Rite Bird] Death Ritual Spear
                new ItemLotGroup(30550), // Mountaintops of the Giants - Field - Great Wyrm Theodorix] Dragon Heart
                new ItemLotGroup(30555), // Consecrated Snowfield - Field - Putrid Avatar] Thorny Cracked Tear
                new ItemLotGroup(30556), // Consecrated Snowfield - Field - Putrid Avatar] Ruptured Crystal Tear
                new ItemLotGroup(30600), // Lake of Rot - Dragonkin Soldier] Dragonscale Blade
                new ItemLotGroup(30620), // Siofra River - Dragonkin Soldier] Dragon Halberd
            };

            SorceryIds = RegulationParams.EquipParamGoods.Where(row => (row.RowName ?? "").StartsWith("[Sorcery]")).Select(row => row.Id).ToArray();
           
            IncantationIds = RegulationParams.EquipParamGoods.Where(row => (row.RowName ?? "").StartsWith("[Incantation]")).Select(row => row.Id).ToArray();
            
            TeardropScarabs = RegulationParams.ItemLotParam_map.Where(row => (row.RowName ?? "").StartsWith("[Teardrop Scarab")).Select(row => new ItemLotGroup(row.Id)).ToArray();
        }
    }
}
