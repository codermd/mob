using System;
using Mxp.Core.Utils;
using System.Collections.ObjectModel;
using System.Linq;
using Mxp.Core.Extensions;

namespace Mxp.Core.Business
{
	public class DynamicComboField : ComboField
	{
		public DynamicFieldHolder DynamicFieldHolder { get; set; }

		private string linkName;

		public DynamicComboField(Model model, DynamicFieldHolder dynamicFieldHolder) : base (model) {
			this.DynamicFieldHolder = dynamicFieldHolder;
			this.Title = this.DynamicFieldHolder.Title;
			this.Type = this.DynamicFieldHolder.LinkType;
			this.linkName = this.DynamicFieldHolder.LinkName;

			this.Choices = new Collection<IComboChoice> ();
			this.DynamicFieldHolder.LookupItems.ForEach (choice => this.Choices.Add (choice));
		}

		public override string VValue {
			get {
				LookupItem lookup = this.GetValue<LookupItem> ();
				return lookup?.Name ?? null;
			}
		}
			
		public override object Value {
			get {
				int? id = (int?)base.Value;

				if (!id.HasValue)
					id = this.DynamicFieldHolder.GetValue<int?> (this.Model, linkName);
				
				return id.HasValue ? this.DynamicFieldHolder.LookupItems.SingleOrDefault (item => item.ComboId == id.Value) : null;
			}
			set {
				base.Value = ((LookupItem)value).ComboId;
				this.DynamicFieldHolder.SetValue (this.Model, ((LookupItem)value).ComboId);
			}
		}
	}
}