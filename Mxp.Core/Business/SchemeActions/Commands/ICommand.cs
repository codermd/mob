using System;
using System.Threading.Tasks;

namespace Mxp.Core.Business
{
	public interface ICommand
	{
		void RedirectToLoginView (ValidationError error = null);
		Task InvokeAsync ();
		void Parse (Uri uri);
	}
}