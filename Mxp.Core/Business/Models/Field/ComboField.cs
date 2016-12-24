using System;
using Mxp.Core.Business;
using System.Collections.ObjectModel;

namespace Mxp.Core.Business
{
	public class ComboField : Field
	{
		public virtual Collection<IComboChoice> Choices { get; set; }

		public ComboField (Model model): base(model) {

		}

		public interface IComboChoice {
			String VTitle { get; }
			int ComboId { get; }
		}
	}
}