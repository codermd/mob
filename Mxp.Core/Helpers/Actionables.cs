using System;
using System.Collections.Generic;

namespace Mxp.Core.Utils
{
	public class Actionables
	{
		public string Title { get; set; }
		public List<Actionable> Actions { get; set; }

		public Actionables (string title, List<Actionable> actions) {
			this.Title = title;
			this.Actions = actions;
		}

		public bool HasActions => !this.Actions.IsEmpty ();
	}
}