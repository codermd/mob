using Android.Views;
using Android.App;
using Mxp.Core.Business;
using Android.Widget;
using Mxp.Core.Helpers;
using Mxp.Droid.Helpers;
using System.ComponentModel;

namespace Mxp.Droid
{
	public abstract class AbstractFieldHolder
	{
		protected Activity mActivity;
		public Field Field { get; set; }

		public Java.Lang.Object Tag { get; set; }

		protected virtual int LayoutResourceId {
			get {
				return Resource.Layout.List_fields_default_item;
			}
		}

		private WeakReferenceObject<BaseAdapter<WrappedObject>> weakParentAdapter;
		public BaseAdapter<WrappedObject> ParentAdapter {
			get {
				return this.weakParentAdapter == null ? null : this.weakParentAdapter.Value;
			}
			private set {
				this.weakParentAdapter = new WeakReferenceObject<BaseAdapter<WrappedObject>> (value);
			}
		}

		public AbstractFieldHolder (BaseAdapter<WrappedObject> parentAdapter, Activity activity) {
			this.ParentAdapter = parentAdapter;
			this.mActivity = activity;
		}

		public virtual View GetView (int position, View convertView, ViewGroup parent, Field field) {
			this.Field = field;

			FieldViewHolder<TextView> viewHolder = null;

			if (convertView == null || this.Tag == null) {
				convertView = this.mActivity.LayoutInflater.Inflate (LayoutResourceId, parent, false);
				viewHolder = new FieldViewHolder<TextView> (convertView);
				convertView.Tag = new JavaObjectHolder<AbstractFieldHolder> (this);
				this.Tag = viewHolder;
			} else
				viewHolder = this.Tag as FieldViewHolder<TextView>;

			viewHolder.BindView (this.Field);

			return convertView;
		}

		public abstract void OnListItemClick (ListView listView, View view, int position, long id);

		protected class FieldViewHolder<V> : ViewHolder<Field> where V : View {
			protected TextView mTitleView;
			protected V mValueView;
			protected ProgressBar mProgressBar;

			private Field mField;

			public FieldViewHolder (View convertView) {
				this.mTitleView = convertView.FindViewById<TextView> (Resource.Id.Key);
				this.mValueView = convertView.FindViewById<V> (Resource.Id.Value);
				this.mProgressBar = convertView.FindViewById<ProgressBar> (Resource.Id.ProgressBar);
			}

			public void BindTitle (string title) {
				if (this.mTitleView != null)
					this.mTitleView.Text = title;
			}

			public override void BindView (Field field) {
				if (this.mField != null)
					this.mField.PropertyChanged -= HandlePropertyChangedEvent;

				this.mField = field;

				this.mField.PropertyChanged += HandlePropertyChangedEvent;

				this.BindTitle (field.VTitle);
				this.RefreshValue (field);
			}

			public virtual void BindValue (Field field) {
				if (typeof (V) == typeof (TextView))
					((TextView)(object)this.mValueView).Text = field.VValue;
			}

			private void HandlePropertyChangedEvent (object sender, PropertyChangedEventArgs e) {
				if (e.PropertyName.Equals ("Loading"))
					this.RefreshValue ((Field)sender);
			}

			private void RefreshValue (Field field) {
				this.mValueView.Visibility = this.mField.IsLoading ? ViewStates.Gone : ViewStates.Visible;

				if (this.mProgressBar != null)
					this.mProgressBar.Visibility = this.mField.IsLoading ? ViewStates.Visible : ViewStates.Gone;

				this.BindValue (field);
			}
		}

		public FieldTypeEnum FieldType {
			get {
				return this.Field.Type;
			}
		}
	}
}