using System;

namespace Mxp.Core.Services.Responses
{
	public class LabelResponse : Response
	{
		public int DictionaryID { get; set; }
		public string DicoLabel { get; set; }

		public LabelResponse () {}
	}
}