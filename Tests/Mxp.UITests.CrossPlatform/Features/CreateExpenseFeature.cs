using Mxp.UITests.CrossPlatform.Features;
using Mxp.UITests.CrossPlatform.Helpers;
using Xamarin.UITest;

namespace Mxp.UITests.CrossPlatform
{
    public partial class CreateExpenseFeature : FeatureBase
    {
        public CreateExpenseFeature(Platform platform, string iOSSimulator) : base(platform, iOSSimulator)
        { }

        public CreateExpenseFeature() : base(Platform.Android, "") { }

        protected override void TearDownTestPassed()
        {
            var helper = TestHelperBase.Create();
            helper.DeleteExpense();
            base.TearDownTestPassed();
        }
    }
}
