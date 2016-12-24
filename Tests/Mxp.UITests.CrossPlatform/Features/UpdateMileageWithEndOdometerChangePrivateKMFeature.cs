using Mxp.UITests.CrossPlatform.Features;
using Mxp.UITests.CrossPlatform.Helpers;
using Xamarin.UITest;

namespace Mxp.UITests.CrossPlatform
{ 
    public partial class UpdateMileageWithEndOdometerChangePrivateKMFeature : FeatureBase
    {
        public UpdateMileageWithEndOdometerChangePrivateKMFeature(Platform platform, string iOSSimulator) : base(platform, iOSSimulator)
        { }

        public UpdateMileageWithEndOdometerChangePrivateKMFeature() : base(Platform.Android, "") { }

        protected override void TearDownTestPassed()
        {
            var helper = TestHelperBase.Create();
            helper.CancelExpense();
            base.TearDownTestPassed();
        }
    }
}
