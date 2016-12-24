using Mxp.Core.Business;
using Mxp.Core.Services.Google;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mxp.Win
{
    public class PredictionResultItem
    {
        public PredictionResultItem()
        {
            items = new List<Item>();
        }
        public class Item
        {
            public Item(Prediction item) { PredictionItem = item; }

            public Prediction PredictionItem { get; set; }


            public override string ToString()
            {
                return PredictionItem.description;
            }
        }
        public List<Item> items { get; set; }
    }
}