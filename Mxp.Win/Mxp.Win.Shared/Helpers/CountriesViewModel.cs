using Mxp.Core.Business;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Diagnostics;

namespace Mxp.Win
{
    public class CountriesViewModel
    {
        private List<IGrouping<string, Country>> _itemsByGroup = new List<IGrouping<string, Country>>();
        public List<IGrouping<string, Country>> ItemsByGroup { get { return _itemsByGroup; } }
        public CountriesViewModel(String filter = null, Field field = null)
        {
            if (String.IsNullOrEmpty(filter))
            {
                if (field != null && field is AttendeeFormCountry)
                    _itemsByGroup = ((Attendee)((AttendeeFormCountry)field).Model).Countries.GroupedCountries;
                else
                    _itemsByGroup = LoggedUser.Instance.Countries.GroupedCountries;
            }
            else
            {
                if (field != null && field is AttendeeFormCountry)
                    _itemsByGroup = ((Attendee)((AttendeeFormCountry)field).Model).Countries.SearchWith(filter).GetGroupedCountries(true);
                else
                    _itemsByGroup = LoggedUser.Instance.Countries.SearchWith(filter).GetGroupedCountries(true);
            }
        }
    }
}

