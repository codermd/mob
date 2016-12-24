using Mxp.UITests.CrossPlatform.Features;
using Mxp.UITests.CrossPlatform.Helpers;
using Xamarin.UITest;

namespace Mxp.UITests.CrossPlatform
{
    public partial class ModifyMileageKMFeature : FeatureBase
    {
        public ModifyMileageKMFeature(Platform platform, string iOSSimulator) : base(platform, iOSSimulator)
        { }

        public ModifyMileageKMFeature() : base(Platform.Android, "") { }

        protected override void TearDownTestPassed()
        {
            var helper = TestHelperBase.Create();

			if (this.Platform == Platform.iOS)
            helper.SelectFirstMileage();
            helper.DeleteMilleagee();
            base.TearDownTestPassed();
        }
    }
}
