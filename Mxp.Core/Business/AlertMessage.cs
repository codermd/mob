using System;

namespace Mxp.Core
{
	public class AlertMessage {
		public enum MessageTypeEnum {
			StartLoading,
			StopLoading,
			Error
		}

		public MessageTypeEnum MessageType { get; }
		public string Title { get; }
		public string Message { get; }

		public AlertMessage (MessageTypeEnum messageType, string title = null, string message = null) {
			this.MessageType = messageType;
			this.Title = title;
			this.Message = message;
		}

		public class AlertMessageEventArgs : EventArgs {
			public AlertMessage AlertMessage { get; set; }

			public AlertMessageEventArgs (AlertMessage message) {
				this.AlertMessage = message;
			}
		}
	}
}