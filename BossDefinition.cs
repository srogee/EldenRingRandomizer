using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EldenRingItemRandomizer
{
    public class BossDefinition
    {
        public string Name;
        public int[] MapItemLotIds;

        public BossDefinition(string name, params int[] mapItemLotIds)
        {
            Name = name;
            MapItemLotIds = mapItemLotIds;
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
    }
}
