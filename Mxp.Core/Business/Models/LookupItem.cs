using System;

using Mxp.Core.Services.Responses;
using Mxp.Core.Extensions;

namespace Mxp.Core.Business
{
	public class LookupItem : Model, ComboField.IComboChoice
	{
		public virtual string Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }

		public LookupItem () {
			
		}

		public LookupItem (LookupItemResponse lookupItemResponse) {
			this.Id = lookupItemResponse.Id;
			this.Name = lookupItemResponse.Name;
			this.Description = lookupItemResponse.Description;
		}

		public virtual string VTitle {
			get {
				return this.Description;
			}
		}

		public int ComboId {
			get {
				return Convert.ToInt32 (this.Id);
			}
		}

		public static NoSelectionLookupItem Empty { get; set; } = new NoSelectionLookupItem ();

		public class NoSelectionLookupItem : LookupItem
		{
			public NoSelectionLookupItem () {}

			public override string VTitle {
				get {
					return Labels.GetLoggedUserLabel (Labels.LabelEnum.NoSelection);
				}
			}

			public override string Id {
				get {
					return default (int).ToString ();
				}
			}
		}
	}
}