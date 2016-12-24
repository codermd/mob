namespace Mxp.Core.Business
{
	public partial class SpendCatcherExpense
	{
		public string VDate {
			get {
				return this.Date.ToString ("dd/MM/yy - hh:mm");
			}
		}
	}
}
