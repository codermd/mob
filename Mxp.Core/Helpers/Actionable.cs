using System;
using Mxp.Core.Business;

namespace Mxp.Core.Utils
{
	public class Actionable
	{
		public string Title { get; set; }
		public Action Action { get; set; }

		public bool IsConfirmationNeeded { get; set; }
		public Action OnFinish { get; set; }

		public Actionable (string title) {
			this.Title = title;
		}

		public Actionable (string title, Action action) : this (title) {
			this.Action = action;
		}
	}
}