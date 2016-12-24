using System;

namespace Mxp.Core.Services.Google
{
	public class Prediction
	{
		// public string searchstring { get; set; }
		public string description { get; set; }
		// public List<MatchedSubstring> matched_substrings { get; set; }
		public string place_id { get; set; }
		// public List<Term> terms { get; set; }
		// public List<string> types { get; set; }

		public Prediction () {

		}

		public override bool Equals (object obj) {
			if (ReferenceEquals (null, obj))
				return false;

			if (ReferenceEquals (this, obj))
				return true;

			if (obj.GetType () != GetType ())
				return false;

			return this.description.Equals (((Prediction)obj).description);
		}
	}
}