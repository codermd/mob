using Mxp.UITests.CrossPlatform.Features;
using Mxp.UITests.CrossPlatform.Helpers;
using Xamarin.UITest;

namespace Mxp.UITests.CrossPlatform
{
    public partial class UpdateMileageEndOdometerFeature : FeatureBase
    {
        public UpdateMileageEndOdometerFeature(Platform platform, string iOSSimulator) : base(platform, iOSSimulator)
        { }

        public UpdateMileageEndOdometerFeature() : base(Platform.Android, "") { }

        protected override void TearDownTestPassed()
        {
            var helper = TestHelperBase.Create();
            helper.CancelExpense();
            base.TearDownTestPassed();
        }
    }
}
