using System;
using UIKit;
using Mxp.Core.Business;

namespace Mxp.iOS
{
	public class DoneToolBar : UIToolbar
	{

		public event EventHandler ClickOnButton = delegate {};

		private UIBarButtonItem doneButton;

		public DoneToolBar (string title = null): base()
		{
			if (title == null) {
				title = Labels.GetLoggedUserLabel (Labels.LabelEnum.Done);
			}
			this.doneButton = new UIBarButtonItem(title, UIBarButtonItemStyle.Done, delegate(object sender, EventArgs e) {
				if(this.ClickOnButton != null) {
					this.ClickOnButton(this, new EventArgs());
				}
			});

			UIBarButtonItem flex = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace);

			UIBarButtonItem[] buttons = new UIBarButtonItem[]{flex, doneButton };

			this.SetItems (buttons, false);

			this.BackgroundColor = UIColor.LightGray;
		}


	}
}