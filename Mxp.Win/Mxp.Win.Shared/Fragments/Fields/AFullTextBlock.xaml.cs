using System.Text.RegularExpressions;
using Mxp.Core.Business;

namespace Mxp.Win
{
    public sealed partial class AFullTextBlock
    {
        public AFullTextBlock(Field field)
        {
            this.InitializeComponent();
            this.CellField = field;
            this.SetVisual ();
        }
        public void SetVisual () {
            this.TextBlockField.Text = this.CellField.VValue?.Replace ("<BR>","\r") ?? string.Empty;
        }
        public Field CellField { get; set; }
    }
}
