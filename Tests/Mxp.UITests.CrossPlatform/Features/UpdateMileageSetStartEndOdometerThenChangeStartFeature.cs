using Mxp.UITests.CrossPlatform.Features;
using Mxp.UITests.CrossPlatform.Helpers;
using Xamarin.UITest;

namespace Mxp.UITests.CrossPlatform
{
    public partial class UpdateMileageSetStartEndOdometerThenChangeStartFeature : FeatureBase
    {
        public UpdateMileageSetStartEndOdometerThenChangeStartFeature(Platform platform, string iOSSimulator) : base(platform, iOSSimulator)
        { }

        public UpdateMileageSetStartEndOdometerThenChangeStartFeature() : base(Platform.Android, "") { }

        protected override void TearDownTestPassed()
        {
            var helper = TestHelperBase.Create();
            helper.CancelExpense();
            base.TearDownTestPassed();
        }
    }
}
