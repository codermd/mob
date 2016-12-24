using Mxp.UITests.CrossPlatform.Features;
using Xamarin.UITest;

namespace Mxp.UITests.CrossPlatform
{
    public partial class DeleteExpenseFeature : FeatureBase
    {
        public DeleteExpenseFeature(Platform platform, string iOSSimulator) : base(platform, iOSSimulator)
        { }

        public DeleteExpenseFeature() : base(Platform.Android, "") { }
    }
}
