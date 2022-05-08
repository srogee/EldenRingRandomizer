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
        private HashSet<int> ChosenWeapons;
        private HashSet<int> ChosenGoods;
        private HashSet<int> ChosenAccessories;
        private List<int> SpiritAshIds;
        private List<int> SorceryIds;
        private List<int> IncantationIds;

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

        public class ItemLotGroup
        {
            public List<int> ItemLotIds;
            public ItemLotGroup(params int[] itemLotIds)
            {
                ItemLotIds = new List<int>(itemLotIds);
            }
        }

        private int[] Talismans = new int[]
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
        private int[] Tears = new int[]
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

        private Random RandomNumberGenerator;

        private List<ItemLotGroup> Churches = new List<ItemLotGroup>(new ItemLotGroup[] {
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
        });

        private List<ItemLotGroup> MajorBosses = new List<ItemLotGroup>(new ItemLotGroup[] {
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
        });

        private List<ItemLotGroup> MinorBosses = new List<ItemLotGroup>(new ItemLotGroup[] {
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
        });

        private List<ItemLotGroup> TeardropScarabs;

        public void Run()
        {
            SpiritAshIds = new List<int>();
            for (int i = 0; i <= 63; i++)
            {
                SpiritAshIds.Add(200000 + i * 1000);
            }

            ChosenWeapons = new HashSet<int>();
            ChosenGoods = new HashSet<int>();
            ChosenAccessories = new HashSet<int>();
            TeardropScarabs = new List<ItemLotGroup>();

            Console.WriteLine($"Loading regulation file...");
            RegulationParams = RegulationParams.Load(ParamClassGenerator.RegulationInPath);

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
                ProcessItemLot(itemLot);
            }

            // Add back in good items
            SorceryIds = RegulationParams.EquipParamGoods.Where(row => (row.RowName ?? "").StartsWith("[Sorcery]")).Select(row => row.Id).ToList();
            IncantationIds = RegulationParams.EquipParamGoods.Where(row => (row.RowName ?? "").StartsWith("[Incantation]")).Select(row => row.Id).ToList();
            TeardropScarabs = RegulationParams.ItemLotParam_map.Where(row => (row.RowName ?? "").StartsWith("[Teardrop Scarab")).Select(row => new ItemLotGroup(row.Id)).ToList();
            RandomizeItemGroups(Churches);
            RandomizeItemGroups(MinorBosses);
            RandomizeItemGroups(MajorBosses);
            RandomizeItemGroups(TeardropScarabs);
            AddNiceStartingItems();
            GivePlayerMaxFlasks();

            Console.WriteLine($"Modifying shop inventories...");
            foreach (var shopLineup in RegulationParams.ShopLineupParam)
            {
                ProcessShopLineup(shopLineup);
            }
            AddItemsToTwinMaidenHusks();

            ChosenWeapons.Clear(); // Reset chosen weapons so starting classes aren't locked out of some weapons
            ChosenGoods.Clear();
            ChosenAccessories.Clear();
            ModifyStartingGifts();
            ModifyStartingClasses();

            Console.WriteLine($"Saving regulation file...");
            RegulationParams.Save(ParamClassGenerator.RegulationOutPath);

            Console.WriteLine($"Done");
        }

        private void AddNiceStartingItems()
        {
            var row = RegulationParams.ItemLotParam_map[18000081];
            row.ItemID1 = 1110; // Gold scarab talisman
            row.ItemCategory1 = ItemlotItemcategory.Accessory;
        }

        private void RandomizeItemGroups(IEnumerable<ItemLotGroup> groups)
        {
            foreach (var group in groups)
            {
                foreach (var itemLotId in group.ItemLotIds)
                {
                    var itemLot = RegulationParams.ItemLotParam_map[itemLotId];
                    var itemId = GetRandomGoodItem(out ItemlotItemcategory category);
                    itemLot.ItemID1 = itemId;
                    itemLot.ItemCategory1 = category;
                    itemLot.ItemChance1 = 1000;
                    itemLot.ItemAmount1 = 1;
                }
            }
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
            // TODO
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

        private int ChooseWeaponOfType(WepType[] types)
        {
            // Choose type, then weapon in type
            // More biased, but greater variety of weapon types.
            EquipParamWeapon[] potentialWeapons = null;
            WepType[] typesCopy = types.ToArray();
            Shuffle(typesCopy);

            foreach (var type in typesCopy)
            {
                potentialWeapons = RegulationParams.EquipParamWeapon.Where(weapon => weapon.RowName?.Length > 0 && weapon.WeaponType == type && !ChosenWeapons.Contains(weapon.Id)).ToArray();
                if (potentialWeapons.Length == 0)
                {
                    continue;
                }
            }

            if (potentialWeapons != null && potentialWeapons.Length > 0)
            {
                var index = RandomNumberGenerator.Next(0, potentialWeapons.Length);
                var weapon = potentialWeapons[index];
                ChosenWeapons.Add(weapon.Id);
                return WeaponMaxUpgradeIdDict[weapon];
            }
            else
            {
                Console.WriteLine($"Error - ran out of weapons {string.Join(", ", types)}");
                return -1;
            }
        }

        private int GetRandomGoodItem(out ItemlotItemcategory category)
        {
            var choice = RandomNumberGenerator.Next(0, 4 + 1);
            switch (choice)
            {
                case 0:
                    category = ItemlotItemcategory.Good;
                    return GetRandomSpiritAsh();
                case 1:
                    category = ItemlotItemcategory.Accessory;
                    return GetRandomTalisman();
                case 2:
                    category = ItemlotItemcategory.Good;
                    return GetRandomTear();
                case 3:
                    category = ItemlotItemcategory.Good;
                    if (RandomNumberGenerator.Next(0, 2) == 0)
                    {
                        return GetRandomSorcery();
                    } else
                    {
                        return GetRandomIncantation();
                    }
                default:
                    category = ItemlotItemcategory.Weapon;
                    return GetRandomWeapon();
            }

            return -1;
        }

        private int GetRandomWeapon()
        {
            return ChooseWeaponOfType(GoodWeaponTypes);
        }

        private int GetRandomSpiritAsh() => SimpleChooseItem(SpiritAshIds, ChosenGoods);
        private int GetRandomTalisman() => SimpleChooseItem(Talismans, ChosenAccessories);
        private int GetRandomTear() => SimpleChooseItem(Tears, ChosenGoods);
        private int GetRandomSorcery() => SimpleChooseItem(SorceryIds, ChosenGoods);
        private int GetRandomIncantation() => SimpleChooseItem(IncantationIds, ChosenGoods);

        private int SimpleChooseItem(IEnumerable<int> potentialItemIds, HashSet<int> alreadyChosenIds)
        {
            var filteredItemIds = potentialItemIds.Where(id => !alreadyChosenIds.Contains(id)).ToArray();
            if (filteredItemIds.Length > 0)
            {
                var index = RandomNumberGenerator.Next(0, filteredItemIds.Length);
                var id = filteredItemIds[index];
                alreadyChosenIds.Add(id);
                return filteredItemIds[index] + 10;
            }
            else
            {
                return 0;
            }
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

                startingClass.EquippedAccessorySlot1 = GetRandomTalisman();
                startingClass.EquippedAccessorySlot2 = GetRandomTalisman();

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

                // Flash of wondrous physick
                startingClass.EquippedItemSlot1 = 251;
                startingClass.EquippedItemSlot1Amount = 1;

                // Lantern
                startingClass.EquippedItemSlot2 = 2070;
                startingClass.EquippedItemSlot2Amount = 1;

                // Stonesword key
                startingClass.EquippedItemSlot3 = 8000;
                startingClass.EquippedItemSlot3Amount = 10;

                // Spirit ashes
                // TODO - consider making this only give bad spirit ashes
                startingClass.EquippedItemSlot4 = GetRandomSpiritAsh();
                startingClass.EquippedItemSlot4Amount = 1;

                // Tears
                startingClass.EquippedItemSlot5 = GetRandomTear();
                startingClass.EquippedItemSlot5Amount = 1;
                startingClass.EquippedItemSlot6 = GetRandomTear();
                startingClass.EquippedItemSlot6Amount = 1;

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
            var staveUsers = new WepType[]
            {
                WepType.StraightSword,
                WepType.Staff
            };

            var sealUsers = new WepType[]
            {
                WepType.StraightSword,
                WepType.Seal
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
            PickStartingClassWeapons(RegulationParams.CharaInitParam[3004], staveUsers, shields);
            // Prophet
            PickStartingClassWeapons(RegulationParams.CharaInitParam[3005], sealUsers, shields);
            // Confessor
            PickStartingClassWeapons(RegulationParams.CharaInitParam[3006], sealUsers, shields);
            // Samurai
            PickStartingClassWeapons(RegulationParams.CharaInitParam[3007], staveUsers, shields);
            // Prisoner
            PickStartingClassWeapons(RegulationParams.CharaInitParam[3008], moreDexBasedMeleeWeapons, shields);
            // Wretch
            PickStartingClassWeapons(RegulationParams.CharaInitParam[3009], allMeleeWeapons, shields);
        }

        void PickStartingClassWeapons(CharaInitParam startingClass, WepType[] rightHand, WepType[] leftHand)
        {
            var howManyRight = 3;
            var howManyLeft = 1;
            string[] slots = new string[] { "Primary", "Secondary", "Tertiary" };

            for (int i = 0; i < howManyRight; i++)
            {
                var weaponId = ChooseWeaponOfType(rightHand);
                startingClass[$"EquippedWeaponRight{slots[i]}"].Value = weaponId;
            }

            for (int i = 0; i < howManyLeft; i++)
            {
                var weaponId = ChooseWeaponOfType(leftHand);
                startingClass[$"EquippedWeaponLeft{slots[i]}"].Value = weaponId;
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

                var itemId = (int)itemIdCell.Value;
                var category = (ItemlotItemcategory)categoryCell.Value;

                // Key item, unique item, or serpent-hunter
                if ((category == ItemlotItemcategory.Good && IsKeyItemOrUniqueItem(itemId)) || (category == ItemlotItemcategory.Weapon && itemId == 17030000))
                {
                    continue;
                }

                itemIdCell.Value = 0;
                itemChanceCell.Value = 0;
                amountCell.Value = 0;
                categoryCell.Value = ItemlotItemcategory.None;

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