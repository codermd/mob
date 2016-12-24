using System;
using Mxp.Core.Extensions;
using Mxp.Utils;

namespace Mxp.Core.Business
{
	public class DynamicLookupField : LookupField
	{
		public DynamicFieldHolder DynamicFieldHolder { get; set; }

		private string linkName;

		public DynamicLookupField(Model model, DynamicFieldHolder dynamicFieldHolder) : base (model) {
			this.DynamicFieldHolder = dynamicFieldHolder;
			this.Title = this.DynamicFieldHolder.Title;
			this.Type = this.DynamicFieldHolder.LinkType;
			this.linkName = this.DynamicFieldHolder.LinkName;
		}

		public override string VValue {
			get {
				if (this.Value != null && this.Value is String && !this.GetValue<String> ().IsInt ())
					return (String)Value;

				return base.VValue;
			}
		}

		public override object Value {
			get {
				object value = base.Value;

				if (value != null)
					return value;

				value = this.DynamicFieldHolder.GetValue (this.Model, this.linkName);

				return value;
			}
			set {
				base.Value = value;
				this.DynamicFieldHolder.SetValue (this.Model, base.Value);
			}
		}
	}
}