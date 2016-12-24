using System;

using Mxp.Core.Services;
using Mxp.Core.Services.Responses;

namespace Mxp.Core.Business
{
	public class Receipt : Model
	{
		public int AttachmentId { get; set; }
		public int ObjectId { get; set; }
		public string ObjectType { get; set; }
		public string AttachmentType { get; set; }
		public string AttachmentOriginalName { get; set; }
		public string AttachmentPath { get; set; }

		public Receipt () {

		}

		public Receipt (ReceiptResponse receiptResponse) {
			this.Populate (receiptResponse);
		}

		public void Populate(ReceiptResponse receiptResponse) {
			this.base64 = null;

			this.AttachmentId = receiptResponse.AttachmentID;
			this.ObjectId = receiptResponse.ObjectID;
			this.ObjectType = receiptResponse.ObjectType;
			this.AttachmentType = receiptResponse.AttachmentType;
			this.AttachmentOriginalName = receiptResponse.AttachmentOriginalName;
			Uri result;
			Uri.TryCreate (new Uri (Service.BaseUrl), receiptResponse.AttachmentPath, out result);
			this.AttachmentPath = result != null ? result.ToString () : null;
		}

		public bool IsDocument {
			get {
				return this.AttachmentType != "image/jpg" && this.AttachmentType != "image/jpeg" && this.AttachmentType != "image/png";
			}
		}
			
		public string base64;
		public Receipt (string base64) {
			this.base64 = base64;
			this.AttachmentType = "image/jpeg";
		}
	}
}