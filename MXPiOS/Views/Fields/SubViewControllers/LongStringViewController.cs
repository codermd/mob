
using System;
using CoreGraphics;

using Foundation;
using UIKit;
using Mxp.Core.Business;

namespace Mxp.iOS
{
	public partial class LongStringViewController : MXPViewController
	{
		private LongStringDataFieldCell dataField;

		public void setDataField(LongStringDataFieldCell dataField) {
			this.dataField = dataField;
		}

		public LongStringViewController () : base ("LongStringViewController", null) {

		}

		public override void ViewDidLoad () {
			base.ViewDidLoad ();
			this.EdgesForExtendedLayout = UIRectEdge.None;
			this.Title = this.dataField.Field.VTitle;
		}


		public override void ViewDidAppear (bool animated) {
			base.ViewDidAppear (animated);
			this.NavigationItem.SetHidesBackButton (false, animated);
		}

		public override void ViewWillAppear (bool animated) {
			base.ViewWillAppear (animated);

			this.TextView.Text = this.dataField.Field.VValue;
			this.TextView.BecomeFirstResponder ();

			this.TextView.Editable = this.dataField.Field.IsEditable;

			this.TextView.Changed += (object sender, EventArgs e) => {
				if (this.TextView.Text != this.dataField.Field.GetValue<String>())
					this.dataField.Field.Value = this.TextView.Text;
			};
		}
	}
}