using System;

namespace Mxp.Core.Business
{
	public interface ICommandsFactory
	{
		ICommand GetCommand (SchemeActionStrategy.CommandTypeEnum commandType, Uri uri);
	}
}