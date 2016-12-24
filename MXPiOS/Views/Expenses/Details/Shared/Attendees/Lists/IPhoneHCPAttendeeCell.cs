using System;

using Foundation;
using UIKit;
using Mxp.Core.Business;

namespace Mxp.iOS
{
	public partial class IPhoneHCPAttendeeCell : UITableViewCell
	{
		public static readonly NSString Key = new NSString ("IPhoneHCPAttendeeCell");
		public static readonly UINib Nib;

		static IPhoneHCPAttendeeCell ()
		{
			Nib = UINib.FromName ("IPhoneHCPAttendeeCell", NSBundle.MainBundle);
		}

		public IPhoneHCPAttendeeCell (IntPtr handle) : base (handle)
		{
		}

		public void Configure(Attendee attendee) {
			this.TitleLabel.Text = attendee.VName.ToUpper();
			this.ZipLabel.Text = attendee.Reference;
			this.AdressLabel.Text = attendee.City + ", " + attendee.ZipCode+ ", "+ attendee.State;
		}

		public static IPhoneHCPAttendeeCell Create ()
		{
			return (IPhoneHCPAttendeeCell)Nib.Instantiate (null, null) [0];
		}
	}
}
