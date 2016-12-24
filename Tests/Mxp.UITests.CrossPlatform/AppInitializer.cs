using Mxp.UITests.CrossPlatform.Helpers;
using Xamarin.UITest;

namespace Mxp.UITests.CrossPlatform
{
    public static class AppInitializer
    {
        public static IApp StartApp(Platform platform, string iOSSimulator)
        {
            if (platform == Platform.Android)
            {
                return ConfigureApp.Android.Debug().StartApp();
            }

            //return ConfigureApp.iOS.DeviceIdentifier ("de98c3f0fac40e2c647ffa57518d29a9b93101a6").InstalledApp ("com.mobileexpense.com").DeviceIp ("10.9.8.21").StartApp();
            //return ConfigureApp.iOS.Debug().EnableLocalScreenshots().AppBundle("../../../../MXPiOS/Bin/iPhoneSimulator/Debug/MXPiOS.app").SetDeviceByName(iOSSimulator).StartApp();
            return ConfigureApp.iOS.AppBundle("../../../../MXPiOS/Bin/iPhoneSimulator/Debug/MXPiOS.app").SetDeviceByName(iOSSimulator).StartApp();
        }
    }
}
