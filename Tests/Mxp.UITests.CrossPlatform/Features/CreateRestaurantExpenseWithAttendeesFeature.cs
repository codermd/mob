using Mxp.UITests.CrossPlatform.Features;
using Mxp.UITests.CrossPlatform.Helpers;
using Mxp.UITests.CrossPlatform.Helpers.Requests;
using Xamarin.UITest;

namespace Mxp.UITests.CrossPlatform
{
    public partial class CreateRestaurantExpenseWithAttendeesFeature : FeatureBase
    {
        public CreateRestaurantExpenseWithAttendeesFeature(Platform platform, string iOSSimulator) : base(platform, iOSSimulator)
        { }

        public CreateRestaurantExpenseWithAttendeesFeature() : base(Platform.Android, "") { }

        protected override void TearDownTestPassed()
        {
            var helper = TestHelperBase.Create();
            helper.SelectExpense(new SelectExpenseRequest());
            helper.DeleteExpense();
            base.TearDownTestPassed();
        }
    }
}
