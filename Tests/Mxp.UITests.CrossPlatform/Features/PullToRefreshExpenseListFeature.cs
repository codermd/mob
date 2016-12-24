using Mxp.UITests.CrossPlatform.Features;
using Xamarin.UITest;

namespace Mxp.UITests.CrossPlatform
{
    public partial class PullToRefreshExpenseListFeature : FeatureBase
    {
        public PullToRefreshExpenseListFeature(Platform platform, string iOSSimulator) : base(platform, iOSSimulator)
        { }

        public PullToRefreshExpenseListFeature() : base(Platform.Android, "") { }
    }
}
