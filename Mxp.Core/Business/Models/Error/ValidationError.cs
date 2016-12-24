using System;

namespace Mxp.Core.Business
{
	// TODO
	public enum ValidationErrorType {

	}

	public class ValidationError : Exception
	{
		public int ErrorId { get; set; }
		public string Verbose { get; set; }
		public string ErrorType { get; set; }

		public ValidationError (string errorType, string verbose) {
			this.ErrorType = errorType;
			this.Verbose = verbose;
		}

		public ValidationError (Exception e) {
			this.ErrorType = "Exception";
			this.Verbose = e.Message;
		}

		public override string ToString () {
			return "Validation Error: " + this.Verbose;
		}
	}
}