using System;
using Xamarin.UITest.Queries;

namespace Mxp.UITests.CrossPlatform.Helpers
{
    public interface ITestHelperBase
    {
        TimeSpan DefaultTimeout { get; set; }
        void TapButtonById(string id, int index = 0);
        void TapButtonByLabel(string mark, int index = 0);
        void TakeScreenshot(string text);
        void EnterText(string text, int index = 0);
        void EnterTextByLabel(string label, string text, int index = 0);
        void WaitForTextFieldElement(string label, string error, TimeSpan? timeout = null);
        void WaitForLabelElement(string label, string error, TimeSpan? timeout = null);
        void Wait(TimeSpan waittime, string timeoutmessage = "Timed out...");
        bool IsLabelElementExists(string text);
        bool IsElementExists(string theclass, string text);

        void TapLoginButton();
        void EnterLoginData(string username, string password);
        bool IsLogged(TimeSpan? timeout = null);
    }
}