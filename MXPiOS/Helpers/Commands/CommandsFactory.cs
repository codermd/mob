using System;
using Mxp.Core.Business;
using UIKit;

namespace Mxp.iOS.Helpers
{
	public class CommandsFactory : ICommandsFactory
	{
		private UIViewController viewController;

		public CommandsFactory (UIViewController viewController) {
			this.viewController = viewController;
		}

		public ICommand GetCommand (SchemeActionStrategy.CommandTypeEnum commandType, Uri uri) {
			switch (commandType) {
				case SchemeActionStrategy.CommandTypeEnum.SAML:
					return new SAMLCommand (this.viewController, uri);
				case SchemeActionStrategy.CommandTypeEnum.OpenObject:
					return new OpenObjectCommand (this.viewController, uri);
				default:
					return default (ICommand);
			}
		}
	}
}