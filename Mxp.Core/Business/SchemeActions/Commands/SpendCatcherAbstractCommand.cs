using System;
using System.Threading.Tasks;
using Mxp.Core.Utils;
using System.Collections.Generic;
using System.Linq;

namespace Mxp.Core.Business
{
	public abstract class SpendCatcherAbstractCommand : ICommand
	{
		public const string HostUri = "spendcatcher";

		private static Task CompletedTask = Task.FromResult (true);

		public SpendCatcherAbstractCommand () {
			
		}

		protected abstract void RedirectToSpendCatcherSharingView ();

		#region ICommand

		public abstract void RedirectToLoginView (ValidationError error = null);

		public void Parse (Uri uri) {}

		public Task InvokeAsync () {
			if (!LoggedUser.Instance.IsAuthenticated)
				this.RedirectToLoginView ();
			else
				this.RedirectToSpendCatcherSharingView ();

			return CompletedTask;
		}

		#endregion
	}
}