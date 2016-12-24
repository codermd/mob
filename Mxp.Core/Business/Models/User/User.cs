using System;

namespace Mxp.Core.Business
{
	public class User : Model
	{
		public User () {

		}

		protected string username;
		public virtual string Username {
			get {
				return this.username;
			}
			set {
				this.username = value;
			}
		}
	}
}