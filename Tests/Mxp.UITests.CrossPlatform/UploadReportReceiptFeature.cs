using Mxp.UITests.CrossPlatform.Features;
using Xamarin.UITest;

namespace Mxp.UITests.CrossPlatform
{
    public partial class UploadReportReceiptFeature : FeatureBase
    {
        public UploadReportReceiptFeature(Platform platform, string iOSSimulator) : base(platform, iOSSimulator)
        { }

        public UploadReportReceiptFeature() : base(Platform.Android, "") { }

    }
}
