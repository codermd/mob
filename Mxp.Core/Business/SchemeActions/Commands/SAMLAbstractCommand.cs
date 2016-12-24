using System;
using System.Threading.Tasks;
using Mxp.Core.Services;

namespace Mxp.Core.Business
{
	public abstract class SAMLAbstractCommand : ICommand
	{
		public const string HostUri = "mxpsessionsharedkey";

		public string Token { get; protected set; }

		public ICommand NextCommand { get; set; }

		public SAMLAbstractCommand (Uri uri) {
			this.Parse (uri);
		}

		#region ICommand

		public abstract void Parse (Uri uri);
		public abstract void RedirectToLoginView (ValidationError error = null);

		public async Task InvokeAsync () {
			LoggedUser.Instance.Token = this.Token;

			try {
				await LoggedUser.Instance.RefreshCacheAsync ();
			} catch (ValidationError e) {
				LoggedUser.Instance.Token = null;
				this.RedirectToLoginView (e);
				return;
			} catch (Exception) {
				LoggedUser.Instance.Token = null;
				this.RedirectToLoginView (new ValidationError ("Error", Service.NoConnectionError));
				return;
			}

			await this.RedirectAsync ();
		}

		#endregion

		protected abstract Task RedirectAsync ();
	}
}