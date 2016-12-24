using Mxp.Core.Business;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Mxp.Win
{
    public class CurrenciesViewModel
    {
        private List<IGrouping<string, Currency>> _itemsByGroup = new List<IGrouping<string, Currency>>();
        public List<IGrouping<string, Currency>> ItemsByGroup { get { return _itemsByGroup; } }
        public CurrenciesViewModel(String filter = null)
        {
            if (String.IsNullOrEmpty(filter))
                _itemsByGroup = LoggedUser.Instance.Currencies.GroupedCurrencies;
            else
                _itemsByGroup = LoggedUser.Instance.Currencies.SearchWith(filter).GetGroupedCurrencies(true);
        }
     
    }
}

