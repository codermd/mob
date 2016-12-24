using Mxp.Core.Business;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace Mxp.Win
{
    public sealed partial class DetailView : UserControl
    {
        public DetailView()
        {
            this.InitializeComponent();
        }

        Boolean Loaded = false;
        public void LoadFields(ExpenseItem expenseItem) {
                CollectionFields = expenseItem.DetailsFields;
            if(!Loaded)
                FillList();
        }

        private void FillList()
        {

            List<FieldGroup> items = new List<FieldGroup>();
            if (CollectionFields != null)
            {
                foreach (TableSectionModel col in CollectionFields)
                {
                    List<Field> fields = col.Fields.ToList<Field>();

                    foreach (Field field in fields)
                    {

                        FieldGroup item = new FieldGroup(field, col.Title);
                        items.Add(item);
                    }
                }
            }

            var result = from t in items
                         group t by t.Group into g
                         select new { Key = g.Key, ItemsField = g };
            FieldsListView.ItemsSource = result;
            Loaded = true;

        }
        public List<DetailField> SourceFields { get; set; }



        public Collection<Field> Fields { get; set; }
      

        public Collection<TableSectionModel> CollectionFields { get; set; }

       
    }

}
