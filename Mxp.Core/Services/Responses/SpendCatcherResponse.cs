using System;
using System.Collections.Generic;
﻿using Mxp.Core.Services.Responses;

namespace Mxp.Core.Services.Responses
{
	public class SpendCatcherResponse : Response
	{
		public int fldSpendCatcherInfoId { get; set; }
		public int fldAttachmentId { get; set; }
		public int fldEmployeeId { get; set; }
		public int fldCustomerId { get; set; }
		public int fldCountryId { get; set; }
		public bool fldIsPaidByCC { get; set; }
		public int fldSpendCatcherInfoProductId { get; set; }
		public object fldSpendCatcherInfoLocation { get; set; }
		public int fldTransactionId { get; set; }
		public string fldSpendCatcherInfoCreatedOn { get; set; }
		public string fldAttachmentPath { get; set; }

		public SpendCatcherResponse () {}
	}
}