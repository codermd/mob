using NUnit.Framework;
using TechTalk.SpecFlow;
using Xamarin.UITest;

namespace Mxp.UITests.CrossPlatform.Features
{
    [TestFixture()]
    [TestFixture(Platform.Android, "")]
    //[TestFixture(Platform.iOS, "")] 
    //[TestFixture (Platform.iOS, iPhone5s.OS_8_1)] 
    [TestFixture (Platform.iOS, iPhone5s.OS_8_2)] 

    public abstract class FeatureBase
    {
        protected static IApp App;
        protected Platform Platform;
        protected string IOsSimulator;

        protected FeatureBase(Platform platform, string osSimulator)
        {
            this.IOsSimulator = osSimulator;
            this.Platform = platform;
        }

        [SetUp]
        public void BeforeEachTest()
        {
            App = AppInitializer.StartApp(Platform, IOsSimulator);
            FeatureContext.Current.Add("App", App);
            FeatureContext.Current.Add("Platform", Platform);
        }

        [TearDown]
        public virtual void TearDown()
        {
            if (TestContext.CurrentContext.Result.Status == TestStatus.Passed)
                TearDownTestPassed();
        }

        protected virtual void TearDownTestPassed()
        {
            
        }
    }
}
