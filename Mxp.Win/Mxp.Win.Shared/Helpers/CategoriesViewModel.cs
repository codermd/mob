using Mxp.Core.Business;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Mxp.Win
{
    public class CategoriesViewModel
    {
        private List<IGrouping<string, Product>> _itemsByGroup = new List<IGrouping<string, Product>> ();

        public List<IGrouping<string, Product>> ItemsByGroup {
            get { return _itemsByGroup; }
        }

        public CategoriesViewModel (String filter = null, AListboxUCType type = AListboxUCType.CategoryAll) {
            Products productlist;

            switch (type) {
                    case AListboxUCType.CategoryNonTravel:
                    productlist = LoggedUser.Instance.Products.NonTravelProduct;
                    break;
                case AListboxUCType.CategoryTravel:
                    productlist = LoggedUser.Instance.Products.TravelProduct;
                    break;
                default:
                    productlist = LoggedUser.Instance.Products.ExpenseProducts;
                    break;
            }

            if (String.IsNullOrEmpty (filter))
                _itemsByGroup = productlist.GroupedProducts;
            else
                _itemsByGroup = productlist.SearchWith (filter).GetGroupedProducts (true);

        }
}
}

