using System;
using Mxp.Core.Helpers;
using System.Collections.Generic;
using System.Diagnostics;
using System.ComponentModel;
using Mxp.Utils;
using Mxp.Core.Extensions;

namespace Mxp.Core.Business
{
	public enum FieldTypeEnum {
		Unknown,

		Country,
		Category,
		Amount,
		Currency,

		String,
		LongString,
		FullText,
		AutocompleteString,
		Date,
		Time,
		Decimal,
		Integer,
		Boolean,
		Combo,
		Lookup,

		PolicyRule
    }

	public enum FieldPermissionEnum {
		Optional, 
		Mandatory, 
		Unknown
	}

	public enum PermissionEnum {
		Hidden, 
		Optional, 
		Mandatory,
		MandatoryTravelRequest,
		MandatoryProject,
		MandatoryDepartment,
		Unknown,
	}

	public class Field : INotifyPropertyChanged
	{
		#region Property changed

		public event PropertyChangedEventHandler PropertyChanged;
		public virtual void NotifyPropertyChanged (string name) {
			// Prevent NullPointerException
			PropertyChangedEventHandler handler = this.PropertyChanged;
			if (handler != null)
				handler (this, new PropertyChangedEventArgs (name));
		}

		#endregion

		public event EventHandler<EventArgs> FieldChanged;

		public string Title { get; set; }

		public Dictionary<String, object> extraInfo;

		private object _value;
		public virtual object Value {
			get { 
				return this._value;
			} 
			set {
				this._value = value;
				this.IsLoading = false;
				this.EmitChangeEvent ();
			}
		}

		private bool _isLoading;
		public bool IsLoading {
			get {
				return this._isLoading;
			}
			set {
				this._isLoading = value;
				this.NotifyPropertyChanged ("Loading");
			}
		}

		public FieldTypeEnum Type { get; protected set; }

		public FieldPermissionEnum Permission { get; set; } = FieldPermissionEnum.Optional;
		public virtual bool IsEditable { get; set; }

		private WeakReferenceObject<Model> _weakModel { get; set; }
		public Model Model {
			get {
				return this._weakModel == null ? null : this._weakModel.Value;
			}
			private set {
				this._weakModel = new WeakReferenceObject<Model> (value);
			}
		}

		public Field (Model model) {
			this.Model = model;
			this.extraInfo = new Dictionary<string, object> ();
			this.Model.RegisterField (this);
		}

		protected void EmitChangeEvent() {
			this.FieldChanged?.Invoke (this, new EventArgs ());
		}

		public T GetModel<T> () where T : Model {
			return this.Model != null ? this.Model as T : default (T);
		}

		public virtual T GetValue<T> () {
			return this.Value != null
				? (T)this.Value
					: typeof(T) == typeof(DateTime)
				? (T)(object)DateTime.Now
					: default (T);
		}

		public virtual string VValue {
			get { 
				return this.Value != null ? this.Value.ToString () : null;
			}
		}

		public string VTitle {
			get {
				if (this.Permission == FieldPermissionEnum.Mandatory)
					return "* " + this.Title;

				return this.Title;
			}
		}

		public void TryValidate () {
			if (this.Permission == FieldPermissionEnum.Mandatory
			    && (this.Value == null
			        || (this.Value is String
			            && (String.IsNullOrWhiteSpace((string)this.Value)
			                || ((this.Type == FieldTypeEnum.Combo
			                     || this.Type == FieldTypeEnum.Lookup)
			                    && this.GetValue<string> ().IsNullOrDefaultInt ())))
			        || (this.Value is Int32
		                && ((this.Type == FieldTypeEnum.Combo
			                 || this.Type == FieldTypeEnum.Lookup)
			                && this.GetValue<int> ().IsDefault ()))
		            || (this.Value is LookupItem
		                && this.GetValue<LookupItem> ().Id.IsNullOrDefaultInt ())
		            || (this.Model is Report
		                && this.Type == FieldTypeEnum.Boolean
		                && !this.GetValue<bool>())))
				throw new ValidationError ("ERROR", Labels.GetLoggedUserLabel (Labels.LabelEnum.ErrorValidation) + " : " + this.Title.ToLower());
		}
	}
}