using System;
using Mxp.UITests.CrossPlatform.Features;
using Mxp.UITests.CrossPlatform.Helpers;
using Xamarin.UITest;

namespace Mxp.UITests.CrossPlatform
{
    public partial class UploadReportExpenseReceiptFeature : FeatureBase
    {
        public UploadReportExpenseReceiptFeature(Platform platform, string iOSSimulator) : base(platform, iOSSimulator)
        { }

        public UploadReportExpenseReceiptFeature() : base(Platform.Android, "") { }

        protected override void TearDownTestPassed()
        {
            var helper = TestHelperBase.Create();
			helper.SelectReport(new Helpers.Requests.SelectReportRequest());
            helper.DeleteReport();
			helper.Wait(TimeSpan.FromSeconds(1));
			helper.SelectExpense(new Helpers.Requests.SelectExpenseRequest());
			helper.DeleteExpense();
            base.TearDownTestPassed();
        }

    }
}
