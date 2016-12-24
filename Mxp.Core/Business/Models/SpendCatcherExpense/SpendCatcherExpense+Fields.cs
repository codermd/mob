using System.Collections.ObjectModel;

namespace Mxp.Core.Business
{
    public partial class SpendCatcherExpense
    {
        public Collection<Field> Fields {
			get {
				return new Collection<Field> {
					new SpendCatcherCountryField (this),
					new SpendCatcherCategoryField (this),
					new SpendCatcherCreditCardField (this)
				};
			}
        }
    }
}
