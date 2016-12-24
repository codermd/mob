// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace Mxp.iOS
{
	[Register ("AddExpenseCell")]
	partial class AddExpenseCell
	{
		[Outlet]
		UIKit.UILabel AddExpenseLabel { get; set; }

		[Outlet]
		UIKit.UIView ButtonAddExpense { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (ButtonAddExpense != null) {
				ButtonAddExpense.Dispose ();
				ButtonAddExpense = null;
			}

			if (AddExpenseLabel != null) {
				AddExpenseLabel.Dispose ();
				AddExpenseLabel = null;
			}
		}
	}
}
