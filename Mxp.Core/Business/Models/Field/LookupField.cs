using System;
using Mxp.Core.Services;
using System.Threading.Tasks;
using Mxp.Core.Utils;
using Mxp.Core.Helpers;
using System.Diagnostics;
using System.Threading;
using Mxp.Core.Extensions;
using Mxp.Utils;

namespace Mxp.Core.Business
{
	public class LookupField : Field
	{
		public event EventHandler LookupItemsChanged = delegate {};
		public event EventHandler LookupChanged = delegate {};

		public LookupService.ApiEnum LookupKey { get; set; }
		public LookupItem LoadedItem { get; set; }
		public LookupItems Results { get; set; }

		private CancellationTokenSource cts;

		public LookupField (Model model) : base (model) {
			this.Type = FieldTypeEnum.Lookup;
			this.Results = new LookupItems ();
			this.ResetResults ();
		}

		public override string VValue {
			get {
				return this.LoadedItem != null ? this.LoadedItem.Name : String.Empty;
			}
		}

		public override object Value {
			set {
				LookupItem lookup = (LookupItem)value;
				this.LoadedItem = lookup;
				base.Value = lookup.Id;
			}
		}

		public async Task FetchItems (string searchString) {
			if (this.cts != null)
				this.cts.Cancel ();

			this.cts = new CancellationTokenSource ();

			try {
				if (this.LookupKey != LookupService.ApiEnum.GetLookupProject
					&& this.LookupKey != LookupService.ApiEnum.GetLookupDepartment
					&& this.LookupKey != LookupService.ApiEnum.GetLookupTravelRequests
					&& String.IsNullOrWhiteSpace (searchString))
					await Task.Run (() => this.ResetResults (), this.cts.Token);
				else
					await LookupService.Instance.FetchLookUp (this, searchString, this.cts.Token);

				this.LookupItemsChanged (this, new EventArgs ());
			} catch (Exception) {
				// ignore -> https://github.com/square/okhttp/issues/1517
			}
		}

		public void ResetResults () {
			this.Results.Clear ();
			this.Results.Add (LookupItem.Empty);
		}
			
		public async Task FetchLookupItem () {
			if (this.LoadedItem != null) // || !this.GetValue<String> ().IsInt ()
				return;

			this.LoadedItem = await LookupService.Instance.FetchLookUp (this);

			this.LookupChanged (this, EventArgs.Empty);
		}
	}
}