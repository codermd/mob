using Mxp.Core.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mxp.Win
{
    public class LookupResultItem
    {
        public LookupResultItem()
        {
            items = new List<Item>();
        }
        public class Item
        {
            public Item(LookupItem item) { LUItem = item; }

            public LookupItem LUItem { get; set; }


            public override string ToString()
            {
                return LUItem.VTitle;
            }
        }
        public List<Item> items { get; set; }
    }
}