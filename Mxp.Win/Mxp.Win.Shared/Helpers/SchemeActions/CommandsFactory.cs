using System;
using System.Collections.Generic;
using System.Text;
using Mxp.Core.Business;
using Mxp.Win.Helpers.SchemeActions.Commands;
using Windows.UI.Popups;

namespace Mxp.Win.Helpers.SchemeActions
{
    class CommandsFactory : ICommandsFactory
    {
        public CommandsFactory()
        {
            
        }
        public ICommand GetCommand(SchemeActionStrategy.CommandTypeEnum commandType, Uri uri)
        {
            switch (commandType)
            {
                case SchemeActionStrategy.CommandTypeEnum.SAML:
                    return new SAMLCommand(uri);
                //case SchemeActionStrategy.CommandTypeEnum.OpenObject:
                //    return new OpenObjectCommand(uri);
                default:
                    return default(ICommand);
            }
        }
    }
}
