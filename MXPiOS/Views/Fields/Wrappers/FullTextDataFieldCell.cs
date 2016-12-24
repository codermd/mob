using System.Drawing;
using CoreGraphics;
using Foundation;
using Mxp.Core.Business;
using UIKit;

namespace Mxp.iOS
{
	public class FullTextDataFieldCell : DataFieldCell {
		public FullTextDataFieldCell (Field field) : base (field) {
			this.accessory = UITableViewCellAccessory.None;
		}

		public override UITableViewCell GetCell (UITableView tableView) {
			FullTextFieldCell cell = this.GenerateCellWithNibName<FullTextFieldCell> ("FullTextFieldCell", tableView);
			cell.SetField (this);
			return cell;
		}

		public override int HeightForCell () {
			int total = 44;

			// Deprecated
			//CGSize size = this.Field.VValue.StringSize (UIFont.SystemFontOfSize (17), new CGSize (310, float.MaxValue), UILineBreakMode.WordWrap);

			// Unified API equivalent
			NSString text = new NSString (this.Field.VValue);
			CGRect size = text.GetBoundingRect (
				new CGSize (310, float.MaxValue),
				NSStringDrawingOptions.UsesLineFragmentOrigin,
				new UIStringAttributes {
					ParagraphStyle = new NSMutableParagraphStyle { LineBreakMode = UILineBreakMode.WordWrap },
					Font = UIFont.SystemFontOfSize (17)
				},
				null);

			return total + (int)size.Height;
		}
	}
}