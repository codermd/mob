using System;
using System.Reflection;
using RestSharp.Portable;
using Mxp.Core.Helpers;
using Mxp.Core.Services.Responses;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;

namespace Mxp.Core.Business
{
	public abstract class Model : INotifyPropertyChanged
	{
		public Model () {

		}

		#region Property changed

		public event PropertyChangedEventHandler PropertyChanged;
		public virtual void NotifyPropertyChanged (string name) {
			if (name.Equals ("ResetChanged"))
				this.modifiedFields.Clear ();

			PropertyChangedEventHandler handler = this.PropertyChanged;
			if (handler != null)
				handler (this, new PropertyChangedEventArgs (name));
		}

		// TODO Change to Memento pattern
		public Collection<Field> modifiedFields = new Collection<Field> ();

		public virtual void ResetChanged () {
			this.NotifyPropertyChanged ("ResetChanged");
		}

		public void RegisterField (Field field) {
			field.FieldChanged += HandleFieldChanged;
		}

		public void UnregisterField (Field field) {
			field.FieldChanged -= HandleFieldChanged;
		}

		public virtual void HandleFieldChanged (object sender, EventArgs e) {
			this.modifiedFields.Add (sender as Field);
			this.NotifyPropertyChanged ("IsChanged");
		}

		public virtual bool IsChanged {
			get {
				return this.modifiedFields.Count > 0;
			}
		}

		public void AddModifiedObject (string property) {
			// HACK
			Field field = new Field (this);
			field.Title = property;
			this.modifiedFields.Add (field);
		}

		#endregion

		#region Property reflection

		public object GetPropertyValue (string propertyName) {
			return this.GetPropertyValue<object> (propertyName);
		}

		public T GetPropertyValue<T> (string propertyName) {
			if (propertyName == null)
				return default (T);
			
			return (T) this.GetType ().GetRuntimeProperty (propertyName).GetValue (this);
		}

		public void SetPropertyValue (string propertyName, object value) {
			if (propertyName == null)
				return;

			this.GetType ().GetRuntimeProperty(propertyName).SetValue(this, value);
		}

		#endregion

		#region Service related

		public virtual void Serialize (RestRequest request) {
			// throws new ValidateError ();
		}
			
		public virtual void TryValidate () {
			// throws new ValidateError ();
		}

		public string SerizalizeDate (DateTime date) {
			// Override default serializer (bug on iPad)
			return date.ToString ("dd") + "/" + date.ToString ("MM") + "/" + date.ToString ("yyyy");
		}

		#endregion

		#region Ancestor utils
			
		private WeakReferenceObject<ISGCollection<Model>> _collectionParent { get; set; } 
		public SGCollection<T> GetCollectionParent<T> () where T : Model {
			return this.GetCollectionParent<SGCollection<T>, T> (); 
		}

		public C GetCollectionParent<C, T> () where C : SGCollection<T> where T : Model {
			if (this._collectionParent == null) {
				return null;
			}

			return this._collectionParent.Value as C;
		}

		public void SetCollectionParent<T> (SGCollection<T> collection = null) where T : Model {
			if (collection == null) {
				this._collectionParent = null;
				return;
			}

			this._collectionParent = new WeakReferenceObject<ISGCollection<Model>> (collection);
		}
			
		public virtual Model GetModelParent<T> () where T : Model {
			return this.GetModelParent<T, Model> ();
		}

		public R GetModelParent<T, R> () where T : Model where R : Model {
			SGCollection<T> collection = this.GetCollectionParent<T> ();

			if (collection == null || collection.ParentModel == null) {
				return null;
			}
				
			return collection.ParentModel as R;
		}

		public virtual void RemoveFromCollectionParent<T> () where T : Model {
			if (this.GetCollectionParent<T> () != null && this.GetCollectionParent<T> ().Contains ((T)this)) {
				this.GetCollectionParent<T> ().Remove ((T)this);
				this.GetCollectionParent<T> ().ResetSectionnedExpenses ();
				this.GetCollectionParent<T> ().NotifyPropertyChanged ("Removed");
			}

			this._collectionParent = null;
		}

		#endregion
	}
}