using System;
using CoreGraphics;

using Foundation;
using UIKit;
using Mxp.Core.Business;
using Mxp.Core;

namespace Mxp.iOS
{

	public partial class AutocompleteStringFieldCell : UITableViewCell
	{

		public AutocompleteStringFieldCell(IntPtr handle) : base(handle)
		{
	}

		public DataFieldCell _DataField;

		public void SetField(DataFieldCell aField)
		{

			this.unbind();
			this._DataField = aField as DataFieldCell;
			this._DataField.Field.FieldChanged += HandleFieldChange;

			this.refresh();
		}

		void HandleFieldChange(object sender, EventArgs e)
		{
			this.refresh();
		}

		protected override void Dispose(bool disposing)
		{
			this.unbind();
			base.Dispose(disposing);
		}

		public void unbind()
		{
			if (this._DataField != null)
			{
				this._DataField.Field.FieldChanged -= HandleFieldChange;
			}
		}

		public void refresh()
		{
			this.TitleLabel.Text = this._DataField.Field.VTitle;
			this.ValueLabel.Text = this._DataField.Field.VValue;
		}

	}
}

