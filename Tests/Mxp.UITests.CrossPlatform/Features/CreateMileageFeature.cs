using Mxp.UITests.CrossPlatform.Features;
using Mxp.UITests.CrossPlatform.Helpers;
using Xamarin.UITest;

namespace Mxp.UITests.CrossPlatform
{
    public partial class CreateMileageFeature : FeatureBase
    {
        public CreateMileageFeature(Platform platform, string iOSSimulator) : base(platform, iOSSimulator)
        { }

        public CreateMileageFeature() : base(Platform.Android, "") { }

        protected override void TearDownTestPassed()
        {
            var helper = TestHelperBase.Create();
            helper.DeleteMilleagee();
            base.TearDownTestPassed();
        }
    }
}
