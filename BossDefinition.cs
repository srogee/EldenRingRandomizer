using EldenRingItemRandomizer.GameState;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EldenRingItemRandomizer
{
    public enum BossType
    {
        Legends,
        GreatEnemies,
        FieldBosses
    }

    public class BossDefinition
    {
        public string Name;
        public int[] MapItemLotIds;
        public BossType Type;

        public BossDefinition(string name, BossType type, params int[] mapItemLotIds)
        {
            Name = name;
            MapItemLotIds = mapItemLotIds;
            Type = type;
        }
    }

    public class ItemAndEventId
    {
        public string Name;
        public int Id;
        public ItemlotItemcategory Category;
        public int EventId;

        public ItemAndEventId(string name, int id, ItemlotItemcategory category, int eventId)
        {
            Name = name;
            Id = id;
            Category = category;
            EventId = eventId;
        }

        public ItemSpawnInfo GetItemSpawnInfo()
        {
            return new ItemSpawnInfo(Id, ConvertCategory(Category), 1, 1, 0, 0, -1, EventId);
        }

        public static Category ConvertCategory(ItemlotItemcategory category)
        {
            return category switch
            {
                ItemlotItemcategory.Accessory => GameState.Category.Accessory,
                ItemlotItemcategory.Ash => GameState.Category.Gem,
                ItemlotItemcategory.Good => GameState.Category.Goods,
                ItemlotItemcategory.Protector => GameState.Category.Protector,
                ItemlotItemcategory.Weapon => GameState.Category.Weapons,
            };
        }
    }

    public class SiteOfGrace
    {
        public string Name;
        public int Id;
        public int EventId;

        public SiteOfGrace(string name, int id, int eventId)
        {
            Name = name;
            Id = id;
            EventId = eventId;
        }
    }
}
