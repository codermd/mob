using System;
using RestSharp.Portable;

namespace Mxp.Core.Business
{
	public partial class Report
	{
		public override void Serialize (RestRequest request) {
			if (!this.IsNew)
				request.AddParameter("reportID", this.Id);

			if (this.IsDraft) {
				request.AddParameter("ReportName", this.Name);
				request.AddParameter("Comment", this.Comment);

				this.Expenses.Serialize (request);

				request.AddParameter("departmentId", this.DepartmentId);
				request.AddParameter("projectId", this.ProjectId);
				request.AddParameter("travelRequestId", this.TravelRequestId);

				if (this.GetDynamicField ("Infochar1") != null)
					request.AddParameter ("infoChar1", this.GetDynamicField ("Infochar1").SerializeValue (this));

				if (this.GetDynamicField ("Infochar2") != null)
					request.AddParameter ("infoChar2", this.GetDynamicField ("Infochar2").SerializeValue (this));

				if (this.GetDynamicField ("Infochar3") != null)
					request.AddParameter ("infoChar3", this.GetDynamicField ("Infochar3").SerializeValue (this));

				if (this.GetDynamicField ("Infonum1") != null)
					request.AddParameter ("infoNum1", this.GetDynamicField ("Infonum1").SerializeValue (this));

				if (this.GetDynamicField ("Infonum2") != null)
					request.AddParameter ("infoNum2", this.GetDynamicField ("Infonum2").SerializeValue (this));
			}
		}
	}
}