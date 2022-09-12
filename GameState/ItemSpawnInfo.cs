using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EldenRingItemRandomizer.GameState
{
    public class ItemSpawnInfo
    {
        public int ID { get; }
        public Category Category { get; }
        public int Quantity { get; }
        public int MaxQuantity { get; }
        public int Infusion { get; }
        public int Upgrade { get; }
        public int Gem { get; }
        public int EventID { get; }
        public ItemSpawnInfo(int id, Category category, int quantity, int maxQuantity, int infusion, int upgrade, int gem, int eventId)
        {
            ID = id;
            Category = category;
            Quantity = quantity;
            MaxQuantity = maxQuantity;
            Infusion = infusion;
            Upgrade = upgrade;
            Gem = gem;
            EventID = eventId;
        }
    }

    public enum Category : uint
    {
        Weapons = 0x00000000,
        Protector = 0x10000000,
        Accessory = 0x20000000,
        Goods = 0x40000000,
        Gem = 0x80000000
    }
}
