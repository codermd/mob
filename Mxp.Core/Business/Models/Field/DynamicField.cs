using System;

namespace Mxp.Core.Business
{
	public class DynamicField : Field
	{
		public DynamicFieldHolder DynamicFieldHolder { get; set; }

		protected string linkName;

		public DynamicField(Model model, DynamicFieldHolder dynamicFieldHolder) : base (model) {
			this.DynamicFieldHolder = dynamicFieldHolder;
			this.Title = this.DynamicFieldHolder.Title;
			this.Type = this.DynamicFieldHolder.LinkType;
			this.linkName = this.DynamicFieldHolder.LinkName;
		}

		public bool IsString {
			get {
				return this.Type == FieldTypeEnum.String
					|| this.Type == FieldTypeEnum.LongString;
			}
		}

		public override T GetValue<T> () {
			return this.Value != null
				? (T)this.Value
					: typeof(T) == typeof(DateTime)
				? (T)(object)DateTime.Now
					: default (T);
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
				if (this.IsString && this.DynamicFieldHolder.LinkFieldLength != 0) {
					if (((string)value).Length > this.DynamicFieldHolder.LinkFieldLength) {
						value = ((string)value).Substring (0, this.DynamicFieldHolder.LinkFieldLength);
					}
				}

				this.DynamicFieldHolder.SetValue (this.Model, value);
				base.Value = value;
			}
		}

		public override string VValue {
			get {
				if (this.Value == null)
					return null;
				
				switch (this.Type) {
					case FieldTypeEnum.Date:
						return ((DateTime)this.Value).ToString (@"dd\/MM\/yyyy");
					case FieldTypeEnum.Time:
						return ((TimeSpan)this.Value).ToString (@"hh\:mm");
					case FieldTypeEnum.Decimal:
						return Math.Round ((double)this.Value, 2).ToString ();
				}

				return this.Value.ToString ();
			}
		}
	}
}