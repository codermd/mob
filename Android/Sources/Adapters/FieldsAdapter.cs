using System;

using Android.Widget;

using Mxp.Core.Business;
using System.Collections.Generic;
using Android.App;
using Android.Views;
using com.refractored.components.stickylistheaders;
using Android.Util;
using Android.Content;
using System.Diagnostics;
using Mxp.Droid.Helpers;
using Android.Graphics;
using System.Net;
using System.Threading.Tasks;
using Mxp.Core.Services.Google;
using Mxp.Droid.Utils;
using Mxp.Core.Helpers;
using Android.Locations;
using System.Collections.ObjectModel;
using Mxp.Core.Utils;

namespace Mxp.Droid.Adapters
{
	public class FieldsAdapter : BaseAdapter<WrappedObject>, IChildFragmentManager
	{
		#pragma warning disable 0414
		private static readonly string TAG = typeof(FieldsAdapter).Name;
		#pragma warning restore 0414

		private Activity mActivity;
		private List<WrappedObject> mFields;

		public Android.Support.V4.App.FragmentManager FragmentManager { get; set; }

		public FieldsAdapter (Android.Support.V4.App.FragmentManager fragmentManager, Activity activity, Collection<Field> fields) : base () {
			this.FragmentManager = fragmentManager;
			this.mActivity = activity;
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
				fieldHolder = FieldHolderFactory.GetFieldHolder (this, this.mActivity, field.Type);
			else
				fieldHolder = ((JavaObjectHolder<AbstractFieldHolder>)convertView.Tag).Instance;

			return fieldHolder.GetView (position, convertView, parent, field);
		}

		public void OnListItemClick (ListView listView, View view, int position, long id) {
			((JavaObjectHolder<AbstractFieldHolder>) view.Tag).Instance.OnListItemClick (listView, view, position, id);
		}

		public Android.Support.V4.App.FragmentManager GetChildFragmentManager () {
			return this.FragmentManager;
		}
	}
}