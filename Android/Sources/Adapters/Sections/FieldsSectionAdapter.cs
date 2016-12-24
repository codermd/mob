
using System;
using Mxp.Droid.Adapters;
using Mxp.Core.Business;
using Android.App;
using Android.Views;
using Mxp.Droid.Helpers;
using Android.Widget;
using Mxp.Droid.ViewHolders;
using com.refractored.components.stickylistheaders;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Mxp.Droid.Pagers;
using Mxp.Droid.Utils;

namespace Mxp.Droid
{
	public class FieldsSectionAdapter<P> : AbstractSectionAdapter<WrappedObject, P> where P : BaseSectionAdapter<WrappedObject>
	{
		#pragma warning disable 0414
		private static readonly string TAG = typeof(FieldsSectionAdapter<P>).Name;
		#pragma warning restore 0414

		private List<WrappedObject> mFields;

		public FieldsSectionAdapter (P parentAdapter, Activity activity, Collection<Field> fields, string title) : base (parentAdapter, activity, title) {
			this.mFields = fields.AsWrappedObject ();
		}

		public override long GetItemId (int position) {
			return position;
		}

		public override WrappedObject this[int index] {
			get {
				return this.mFields [index];
			}
		}

		public override int Count {
			get {
				return this.mFields.Count;
			}
		}
			
		public override View GetView (int position, View convertView, ViewGroup parent) {
			AbstractFieldHolder fieldHolder = null;
			Field field = this [position].GetInstance<Field> ();

			if (convertView == null
				|| convertView.Tag == null
				|| !(convertView.Tag is JavaObjectHolder<AbstractFieldHolder>)
				|| ((JavaObjectHolder<AbstractFieldHolder>)convertView.Tag).Instance.FieldType != field.Type)
				fieldHolder = FieldHolderFactory.GetFieldHolder (this.ParentAdapter, this.Activity, field.Type);
			else
				fieldHolder = ((JavaObjectHolder<AbstractFieldHolder>)convertView.Tag).Instance;

			return fieldHolder.GetView (position, convertView, parent, field);
		}

		public override void OnListItemClick (ListView listView, View view, int position, long id) {
			((JavaObjectHolder<AbstractFieldHolder>) view.Tag).Instance.OnListItemClick (listView, view, position, id);
		}
	}
}