using System;
using System.Collections.Generic;
using Mxp.Core.Services;
using Mxp.Core.Services.Responses;
using System.Linq;
using System.Threading.Tasks;

namespace Mxp.Core.Business
{
	public class Products : SGCollection<Product>
	{
		public Products (Model parent = null) : base (parent) {

		}

		public Products (IEnumerable<Product> products) : base (products) {

		}

		public async override Task FetchAsync () {
			await SystemService.Instance.FetchProductsAsync (this);
		}

		public IEnumerable<Response> rawResponses;
		public override void Populate (IEnumerable<Response> collection) {
			this.rawResponses = collection;
			base.Populate (collection);
		}

		private Products _expenseProducts;
		public Products ExpenseProducts {
			get {
				if (this._expenseProducts == null)
					this._expenseProducts = new Products (this.Where (product => product.CanShowInExpense));

				return this._expenseProducts;
			}
		}

		private Products _travelProduct;
		public Products TravelProduct {
			get {
				if (this._travelProduct == null)
					this._travelProduct = new Products (this.ExpenseProducts.Where (product => product.IsTravelProduct));

				return this._travelProduct;
			}
		}

		private Products _nonTravelProduct;
		public Products NonTravelProduct {
			get {
				if (this._nonTravelProduct == null)
					this._nonTravelProduct = new Products (this.ExpenseProducts.Where (product => !product.IsTravelProduct));

				return this._nonTravelProduct;
			}
		}

		public Products SearchWith (string text) {
			return new Products (this.Where (product => product.ExpenseCategory.Name.IndexOf (text, StringComparison.OrdinalIgnoreCase) >= 0));
		}

		#region iOS

		private List<IGrouping<String, Product>> _groupedProducts;
		public List<IGrouping<String, Product>> GroupedProducts {
			get {
				return this.GetGroupedProducts (false);
			}
		}
		public List<IGrouping<String, Product>> GetGroupedProducts (bool whileSearching = false) {
			if (this._groupedProducts == null)
				this._groupedProducts = this.GroupBy (x => x.ExpenseCategory.Name.Substring (0, 1)).OrderBy (grouping => grouping.Key).ToList ();

			return this._groupedProducts;
		}

		public void ResetGroups () {
			this._groupedProducts = null;
		}

		#endregion
	}
}