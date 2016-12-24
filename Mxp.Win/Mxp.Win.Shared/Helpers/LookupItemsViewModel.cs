using Mxp.Core.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace Mxp.Win.Helpers
{

    public class LookupItemsViewModel
    {
        private List<LookupItem> _itemsByGroup = new List<LookupItem>();
        public List<LookupItem> ItemsByGroup { get { return _itemsByGroup; } }

        public LookupItems LUItems { get; private set; }

        public LookupItemsViewModel(Field field, String filter = null)
        {
            //LookupAnItem(filter, field);

        }

        public async Task LookupAnItem(string filter, Field field)
        {
            ItemsByGroup.Clear();
            try {
                await ((LookupField)field).FetchItems(filter);
            } catch (Exception error) {
                MessageDialog messageDialog = new MessageDialog (error.GetExceptionMessage ());
                messageDialog.Commands.Add (new UICommand ("OK", (command) => { }));
                messageDialog.ShowAsync ();
                return;
            }
            LUItems = ((LookupField)field).Results;
            LookupResultItem packagesResult = new LookupResultItem();
            foreach (LookupItem item in LUItems)
            {
                packagesResult.items.Add(new Mxp.Win.LookupResultItem.Item(item));
            }
            foreach(var item in packagesResult.items)
            {
                ItemsByGroup.Add(item.LUItem);
            }
        }
    }
}
