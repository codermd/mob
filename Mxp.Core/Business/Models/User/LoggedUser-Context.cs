using System;
using System.Collections.ObjectModel;
using Mxp.Core.Business;
using System.Collections.Generic;
using Mxp.Core.Services.Responses;
using Mxp.Core.Services;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using RestSharp.Portable;

namespace Mxp.Core.Business
{
	public partial class LoggedUser
	{

		TrackContext _trackContext = new TrackContext();

		public TrackContext TrackContext { 
			get { 
				return this._trackContext;
			}
		}

	}
}