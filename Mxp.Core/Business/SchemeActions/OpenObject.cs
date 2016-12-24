using System;
using Mxp.Utils;
using RestSharp.Portable;
using System.Threading.Tasks;
using Mxp.Core.Services.Responses;
using Mxp.Core.Services;

namespace Mxp.Core.Business
{
	public class OpenObject
	{
		public enum ObjectTypeEnum {
			Unknown,
			Expense,
			Report,
			ApprovalReport,
			ApprovalTravelRequest
		}

//		public enum ReferenceTypeEnum {
//			Unknown,
//			Airplus
//		}

		public ObjectTypeEnum ObjectType { get; }
		public string ReferenceType { get; }
		public string Reference { get; }

		public OpenObject (string objectType, string referenceType, string reference) {
			this.ObjectType = objectType.ToEnum (ObjectTypeEnum.Unknown);
			this.ReferenceType = referenceType;
			this.Reference = reference;
		}

		public OpenObject (int objectType, string referenceType, string reference) {
			this.ObjectType = (ObjectTypeEnum)objectType;
			this.ReferenceType = referenceType;
			this.Reference = reference;
		}

		public void Serialize (RestRequest request) {
			request.AddParameter ("objectType", this.ObjectType);
			request.AddParameter ("referenceType", this.ReferenceType);
			request.AddParameter ("reference", this.Reference);
		}

		public async Task<MetaOpenObject> FetchAsync () {
			this.TryValidate ();
			return await SystemService.Instance.FetchOpenObjectAsync (this);
		}

		public void TryValidate () {
			if (this.ObjectType == ObjectTypeEnum.Unknown)
				throw new ValidationError ("Error", "OpenObject invalide");
		}
	}
}