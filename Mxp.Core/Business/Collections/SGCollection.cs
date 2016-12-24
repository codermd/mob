using System;
using System.Text;
using System.Linq;
using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Mxp.Core.Services.Responses;
using Mxp.Core.Helpers;
using Mxp.Core.Utils;
using System.ComponentModel;
using System.Collections.Specialized;

namespace Mxp.Core.Business
{
	public class SGCollection<T> : ObservableCollection<T>, ISGCollection<T> where T : Model
	{
		#region INotifyPropertyChanged

		public new event PropertyChangedEventHandler PropertyChanged;

		public virtual void NotifyPropertyChanged (string name) {
			PropertyChangedEventHandler handler = this.PropertyChanged;
			if (handler != null)
				handler (this, new PropertyChangedEventArgs (name));

			if (this.ParentModel != null)
				this.ParentModel.NotifyPropertyChanged (name);
		}

		#endregion

		public bool Loaded { get; protected set; }

		public SGCollection (Model model = null) {
			this.ParentModel = model;
			this.Loaded = false;
		}

		public SGCollection (IEnumerable<Response> collection, Model model = null) : this (model) {
			this.Populate (collection);
		}

		public SGCollection (IEnumerable<T> collection, Model model = null) : base (collection) {
			this.ParentModel = model;
			this.Loaded = true;
		}
			
		public virtual V GetInstance<V> (Response itemResponse) where V : T {
			// Code smells ?
			// Other solution : http://stackoverflow.com/questions/840261/passing-arguments-to-c-sharp-generic-new-of-templated-type
			// https://social.msdn.microsoft.com/Forums/en-US/fd43d184-0503-4d4a-850c-999ca58e1444/creating-generic-t-with-new-constraint-that-has-parameters?forum=csharplanguage
			return (V) Activator.CreateInstance (typeof(T), new Object[] { itemResponse });
		}

		public virtual void Populate (IEnumerable<Response> collection) {
			this.ClearItems ();

			if (collection == null)
				return;
			
			collection.ForEach (item => this.AddItem (this.GetInstance<T> (item), false));	
				
			this.Loaded = true;

			this.NotifyPropertyChanged ("IsChanged");
		}

		public void ReplaceWith (IEnumerable<T> collection) {
			this.Clear ();

			if (collection == null)
				return;

			collection.ForEach (item => this.AddItem (item, true));

			this.NotifyPropertyChanged ("IsChanged");
		}

		public new void InsertItem (int index, T item) {
			item.SetCollectionParent (this);

			base.InsertItem (index, item);
		}

		public virtual void AddItem (T item, bool notify = false) {
			if (this.Contains (item))
				return;
			
			this.Add (item);
			item.SetCollectionParent (this);
		}

		public virtual Task FetchAsync () {
			throw new NotSupportedException ();
		}

		private WeakReferenceObject<Model> parentModel { get; set; }
		public Model ParentModel {
			get {
				if (this.parentModel == null)
					return null;

				return this.parentModel.Value;
			}
			set {
				this.parentModel = new WeakReferenceObject<Model> (value);
			}
		}

		public R GetParentModel<R> () where R : Model {
			return this.ParentModel as R;
		}

		public virtual void TryValidate () {
			// throws new ValidationError
		}

		#region iOS

		public virtual void ResetSectionnedExpenses () {

		}

		#endregion
	}
}