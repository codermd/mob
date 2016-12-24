using System;
using System.Collections.Generic;
using System.Text;
using Mxp.Core;
using Mxp.Win.Helpers;

[assembly: Xamarin.Forms.Dependency(typeof(ServiceAppVersion))]

namespace Mxp.Win.Helpers
{
    public class ServiceAppVersion : IServiceAppVersion
    {
        public ServiceAppVersion() { }

        public string AppVersion()
        {
            var version = Windows.ApplicationModel.Package.Current.Id.Version;
            return string.Format("{0}.{1}.{2}",
            version.Major, version.Minor, version.Build);
        }
    }
}
