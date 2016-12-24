using Mxp.UITests.CrossPlatform.Features;
using Mxp.UITests.CrossPlatform.Helpers;
using Xamarin.UITest;

namespace Mxp.UITests.CrossPlatform
{ 
    public partial class UpdateMileageWithEndOdometerChangePrivateKMAndCommutingKMFeature : FeatureBase
    {
        public UpdateMileageWithEndOdometerChangePrivateKMAndCommutingKMFeature(Platform platform, string iOSSimulator) : base(platform, iOSSimulator)
        { }

        public UpdateMileageWithEndOdometerChangePrivateKMAndCommutingKMFeature() : base(Platform.Android, "") { }

        protected override void TearDownTestPassed()
        {
            var helper = TestHelperBase.Create();
            helper.CancelExpense();
            base.TearDownTestPassed();
        }
    }
}
