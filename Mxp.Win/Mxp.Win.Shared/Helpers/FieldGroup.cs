
using Mxp.Core.Business;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Controls;

namespace Mxp.Win
{
    public class FieldGroup
    {
        public FieldGroup(Field f, String g)
        {
            this.Field = f;
            this.Group = g;
        }
        public Field Field { get; set; }
        public String Group { get; set; }       

    }

}

