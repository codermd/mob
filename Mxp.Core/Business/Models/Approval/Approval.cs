using System;
using Mxp.Core.Services.Responses;
using System.Threading.Tasks;
using RestSharp.Portable;

namespace Mxp.Core.Business
{
	public abstract class Approval : Model
	{
		public string Comment { get; set; }
	}
}