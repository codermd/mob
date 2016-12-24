using System;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using Android.Text;
using Android.Views;
using Android.Widget;
using Mxp.Core.Business;
using Mxp.Droid.Adapters;

namespace Mxp.Droid
{
	public class CategoriesListFragment : Fragment {
		public static readonly string EXTRA_PRODUCTS_TYPE = "com.sagacify.mxp.products.type";

		public interface ICategoryItemClicked {
			void OnItemClicked (Product product);
		}

		private Products mProducts;
		private CategoryDialogAdapter mAdapter;
		private ICategoryItemClicked mListener;

		public CategoriesListFragment (Products products) {
			this.mProducts = products;
		}

		public override void OnAttach (Context context) {
			base.OnAttach (context);

			try {
				if (this.TargetFragment != null)
					this.mListener = (ICategoryItemClicked)this.TargetFragment;
				else
					this.mListener = (ICategoryItemClicked)this.Activity;
			} catch (InvalidCastException) {
				if (this.TargetFragment != null)
					throw new InvalidCastException ("Calling fragment " + this.TargetFragment.Class.SimpleName + " must implement ICategoryItemClicked interface");
				else
					throw new InvalidCastException ("Calling activity " + this.Activity.Class.SimpleName + " must implement ICategoryItemClicked interface");
			}
		}

		public override void OnDetach () {
			base.OnDetach ();

			this.mListener = null;
		}

		public override void OnCreate (Bundle savedInstanceState) {
			base.OnCreate (savedInstanceState);

			this.mAdapter = new CategoryDialogAdapter (this.Activity, this.mProducts);
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
			View view = inflater.Inflate (Resource.Layout.List_categories, container, false);

			EditText filterText = view.FindViewById<EditText> (Resource.Id.Search);
			ListView listView = view.FindViewById<ListView> (Resource.Id.List);

			listView.Adapter = this.mAdapter;
			listView.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => {
				mListener.OnItemClicked (this.mAdapter [e.Position]);
			};

			filterText.TextChanged += (object sender, TextChangedEventArgs e) => {
				this.mAdapter.mFilterIsInvoked = !String.IsNullOrWhiteSpace (e.Text.ToString ());
				this.mAdapter.Filter.InvokeFilter (e.Text.ToString ());
			};

			return view;
		}
	}
}
