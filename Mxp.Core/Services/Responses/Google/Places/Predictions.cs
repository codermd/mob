using System;
using System.Collections.Generic;

namespace Mxp.Core.Services.Google
{
	public class Predictions
	{
		public List<Prediction> predictions { get; set; }
		public string status { get; set; }

		public Predictions () {

		}

		public void AddDefault (Prediction prediction) {
			this.predictions.Insert (0, prediction);
		}

		public bool Contains (Prediction prediction) {
			return this.predictions.Contains (prediction);
		}
	}
}