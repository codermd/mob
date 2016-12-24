using System;

using UIKit;
using Mxp.iOS;
using System.Collections.ObjectModel;
using Mxp.Core.Business;

namespace MXPiOS
{
	public partial class AttendeeDetailsViewController : MXPViewController
	{
		public AttendeeDetailsViewController (Attendee attendee) : base ("AttendeeDetailsViewController", null)
		{
			this.Attendee = attendee;
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			// Perform any additional setup after loading the view, typically from a nib.
		}


		Attendee Attendee;

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			TableSectionsSource source = new TableSectionsSource ();
			source.Sections.Add (new SectionFieldsSource (this.Attendee.AllFields, this, null));

			this.TableView.Source = source;
			this.TableView.ReloadData ();
		}


		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}


