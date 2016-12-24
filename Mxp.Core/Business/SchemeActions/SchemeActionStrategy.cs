using System;
using System.Threading.Tasks;

namespace Mxp.Core.Business
{
	public static class SchemeActionStrategy
	{
		public const string SchemeUri = "mobilexpense://";

		public enum CommandTypeEnum {
			SAML,
			OpenObject,
			SpendCatcher
		}

		public static ICommand GetCommandFromFactory (string scheme, ICommandsFactory commandsFactory) {
			if (IsHome (scheme))
				return default (ICommand);

			Uri uri = new Uri (scheme);

			if (String.IsNullOrEmpty (uri.Host)) {
				UriBuilder builder = new UriBuilder (scheme);
				builder.Host = SAMLAbstractCommand.HostUri;
				uri = builder.Uri;
			}

			switch (uri.Host) {
				case SAMLAbstractCommand.HostUri:
					return commandsFactory.GetCommand (CommandTypeEnum.SAML, uri);
				case OpenObjectAbstractCommand.HostUri:
					return commandsFactory.GetCommand (CommandTypeEnum.OpenObject, uri);
				case SpendCatcherAbstractCommand.HostUri:
					return commandsFactory.GetCommand (CommandTypeEnum.SpendCatcher, uri);
				default:
					return default (ICommand);
			}
		}

		public static bool IsHome (string url) {
			return url.Equals (SchemeUri);
		}
	}
}