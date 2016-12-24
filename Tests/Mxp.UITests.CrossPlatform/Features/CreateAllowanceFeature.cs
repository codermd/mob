using Mxp.UITests.CrossPlatform.Features;
using Mxp.UITests.CrossPlatform.Helpers;
using Xamarin.UITest;

namespace Mxp.UITests.CrossPlatform
{
    public partial class CreateAllowanceFeature : FeatureBase
    {
        public CreateAllowanceFeature(Platform platform, string iOSSimulator) : base(platform, iOSSimulator)
        { }

        public CreateAllowanceFeature() : base(Platform.Android, "") { }

        protected override void TearDownTestPassed()
        {
            var helper = TestHelperBase.Create();
            helper.DeleteAllowance();
            base.TearDownTestPassed();
        }
    }
}
