using System;

namespace Mxp.Core.Business
{
	public partial class Attendee
	{
		// FIXME
		public string VName {
			get {
				if (String.IsNullOrWhiteSpace (this.Reference))
					return !String.IsNullOrWhiteSpace (this.Name) ? this.Name : this.Firstname + " " + this.Lastname;

				String NPI = this.Reference;

				if (NPI.Length > 5)
					NPI = this.Reference.Substring (0, 3) + "..." + this.Reference.Substring (this.Reference.Length - 2);

				return String.Format ("{0} ({1})", this.Name, NPI);
			}
		}

		public string VLocation {
			get {
				return String.Format ("{0}{1}{2}",
					this.Address,
					!String.IsNullOrWhiteSpace (this.Address) ? "\n" + this.City : this.City,
					!String.IsNullOrWhiteSpace (this.City) ? ", " + this.State : this.State);
			}
		}

		public string VAmount {
			get {
				if (this.GetModelParent<Attendee, ExpenseItem> () == null)
					return null;

				return this.AmountLC + " " + this.GetModelParent<Attendee, ExpenseItem> ().Currency.Iso;
			}
		}

		public string VHCPSearchLocation {
			get {
				return $"{this.City}, {this.ZipCode}, {this.State}";
			}
		}
	}
}