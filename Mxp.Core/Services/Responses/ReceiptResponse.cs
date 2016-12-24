using System;

namespace Mxp.Core.Services.Responses
{
	public class ReceiptResponse : Response
	{
		public int AttachmentID { get; set; }
		public int ObjectID { get; set; }
		public string ObjectType { get; set; }
		public string AttachmentType { get; set; }
		public string AttachmentOriginalName { get; set; }
		public string AttachmentPath { get; set; }

		public ReceiptResponse () {}
	}
}