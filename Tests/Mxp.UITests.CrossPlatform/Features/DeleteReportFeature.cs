using Mxp.UITests.CrossPlatform.Features;
using Xamarin.UITest;

namespace Mxp.UITests.CrossPlatform
{
    public partial class DeleteReportFeature : FeatureBase
    {
        public DeleteReportFeature(Platform platform, string iOSSimulator) : base(platform, iOSSimulator)
        { }

        public DeleteReportFeature() : base(Platform.Android, "") { }
    }
}
