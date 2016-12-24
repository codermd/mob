using Mxp.UITests.CrossPlatform.Features;
using Mxp.UITests.CrossPlatform.Helpers;
using Xamarin.UITest;

namespace Mxp.UITests.CrossPlatform
{
    public partial class UploadReportReceiptFeature : FeatureBase
    {
        public UploadReportReceiptFeature(Platform platform, string iOSSimulator) : base(platform, iOSSimulator)
        { }

        public UploadReportReceiptFeature() : base(Platform.Android, "") { }

        protected override void TearDownTestPassed()
        {
            var helper = TestHelperBase.Create();
			helper.SelectReport(new Helpers.Requests.SelectReportRequest());
            helper.DeleteReport();
            base.TearDownTestPassed();
        }
    }
}
