using Mxp.UITests.CrossPlatform.Helpers;
using TechTalk.SpecFlow;
using Xamarin.UITest;

namespace Mxp.UITests.CrossPlatform
{
    public abstract class StepsBase
    {
        protected readonly IApp app;

        protected readonly TestHelperBase _testHelper;

        protected StepsBase()
        {
            app = FeatureContext.Current.Get<IApp>("App");
            _testHelper = TestHelperBase.Create();
        }
    }
}
