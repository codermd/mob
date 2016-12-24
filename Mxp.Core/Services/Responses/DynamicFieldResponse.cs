using System;
using System.Collections.Generic;

namespace Mxp.Core.Services.Responses
{
	public class DynamicFieldResponse : Response
	{
		public int FieldLocationId { get; set; }
		public string FieldLocationName { get; set; }
		public string FieldLinkName { get; set; }
		public string FieldLinkType { get; set; }
		public string ComboTypeId { get; set; }
		public string FieldLinkDescription { get; set; }
		public string ComboTypeStoredProc { get; set; }
		public string FieldLinkDefaultValue { get; set; }
		public string FieldLinkNoSelection { get; set; }
		public int FieldLinkFieldLength { get; set; }
		public List<LookupItemResponse> LookupItems { get; set; }

		public DynamicFieldResponse () {}
	}
}