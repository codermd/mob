using System;
using Mxp.Core.Business;
using Android.App;

namespace Mxp.Droid.Helpers
{
	public class CommandsFactory : ICommandsFactory
	{
		private Activity mActivity;

		public CommandsFactory (Activity activity) {
			this.mActivity = activity;
		}

		public ICommand GetCommand (SchemeActionStrategy.CommandTypeEnum commandType, Uri uri) {
			switch (commandType) {
				case SchemeActionStrategy.CommandTypeEnum.SAML:
					return new SAMLCommand (this.mActivity, uri);
				case SchemeActionStrategy.CommandTypeEnum.OpenObject:
					return new OpenObjectCommand (this.mActivity, uri);
				case SchemeActionStrategy.CommandTypeEnum.SpendCatcher:
					return new SpendCatcherCommand (this.mActivity);
				default:
					return default (ICommand);
			}
		}
	}
}