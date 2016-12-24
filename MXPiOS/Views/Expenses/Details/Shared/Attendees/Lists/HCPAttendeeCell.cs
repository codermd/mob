using System;

using Foundation;
using UIKit;
using System.Collections.Generic;
using CoreAnimation;
using CoreGraphics;

namespace Mxp.iOS
{
	public partial class HCPAttendeeCell : UITableViewCell
	{
		public static readonly NSString Key = new NSString ("HCPAttendeeCell");
		public static readonly UINib Nib;

		static HCPAttendeeCell ()
		{
			Nib = UINib.FromName ("HCPAttendeeCell", NSBundle.MainBundle);
		}

		public HCPAttendeeCell (IntPtr handle) : base (handle)
		{
		}

		public static HCPAttendeeCell Create ()
		{
			return (HCPAttendeeCell)Nib.Instantiate (null, null) [0];
		}

		public override void AwakeFromNib ()
		{
			base.AwakeFromNib ();
			listLabels = new List<UILabel>(this.Labels);
		}


		List<UILabel> listLabels;

		List<CALayer> border = new List<CALayer>();
		void ConfigureLabel() {
			
		}


		public void Configure(List<string> labels, bool isHeader = false){
			if (labels.Count != this.listLabels.Count) {
				return;
			}
			for (int i = 0; i < labels.Count; i++) {
				listLabels [i].Text = labels [i];
				if (isHeader) {
					listLabels [i].Font = UIFont.FromName("Avenir-heavy", listLabels [i].Font.PointSize);
				} else {
					listLabels [i].Font = UIFont.FromName("Avenir-book", listLabels [i].Font.PointSize);
				}
			}
		}


		public override void SetNeedsLayout ()
		{
			base.SetNeedsLayout ();
			this.ConfigureLabel ();
		}

	}
}
