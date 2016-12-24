using System;
using System.Linq;
using Mxp.Core.Utils;

namespace Mxp.Core.Business
{
	public partial class Allowance : Expense
	{
		public const int PAYEMENT_METHOD_ID = 13;

		public int? ALLSIprojectId { get; set; }
		public int? ALLSIdepartmentId { get; set; }
		public int? ALLSItrqId { get; set; }

		public double GrossAmountCC { get; set; }

		public override bool IsSplit {
			get {
				return false;
			}
		}

		public AllowanceSegments AllowanceSegments { get; set; }

		public override void ResetChanged () {
			if (this.AllowanceSegments != null)
				this.AllowanceSegments.ForEach (item => item.ResetChanged ());

			base.ResetChanged ();

			this.NotifyPropertyChanged ("IsChanged");
		}
	}
}