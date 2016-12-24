using Mxp.UITests.CrossPlatform.Features;
using Xamarin.UITest;

namespace Mxp.UITests.CrossPlatform
{
    public partial class UploadExpenseReceiptFeature : FeatureBase
    {
        public UploadExpenseReceiptFeature(Platform platform, string iOSSimulator) : base(platform, iOSSimulator)
        { }

        public UploadExpenseReceiptFeature() : base(Platform.Android, "") { }

    }
}
