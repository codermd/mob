using System;
using Mxp.Core.Utils;

namespace Mxp.Core.Business
{
	public partial class Report
	{
		public override void TryValidate () {
			this.Expenses.TryValidate ();
			this.GetMainFields ().ForEach (Field => Field.TryValidate ());
			this.GetAllFields().ForEach (Field => Field.TryValidate ());
		}
	}
}