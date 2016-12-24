using Mxp.UITests.CrossPlatform.Features;
using Xamarin.UITest;

// ReSharper disable once CheckNamespace
namespace Mxp.UITests.CrossPlatform
{
    public partial class LoginFeature : FeatureBase
    {
        public LoginFeature(Platform platform, string iOSSimulator) : base(platform, iOSSimulator)
        { }

        public LoginFeature() : base(Platform.Android, "") { }
    }
}
