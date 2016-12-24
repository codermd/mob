using System;

using Mxp.Core.Services.Responses;

namespace Mxp.Core.Business
{
	public class Label : Model
	{
		public string DictionaryLabel { get; set; }
		public int DictionaryId { get; set; }

		public Label () {

		}

		public Label (LabelResponse labelResponse) {
			this.DictionaryLabel = labelResponse.DicoLabel;
			this.DictionaryId = labelResponse.DictionaryID;
		}
	}
}