using System;
using Microsoft.VisualStudio.TestTools.UITesting;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Microsoft.VisualStudio.TestTools.UITest.Input;
using Microsoft.VisualStudio.TestTools.UITest.Extension;
using Microsoft.VisualStudio.TestTools.UITesting.WindowsRuntimeControls;


namespace Mxp.UITests.WindowsPhone
{
    /// <summary>
    /// Summary description for CodedUITest2
    /// </summary>
    [CodedUITest]
    public class CodedUITest2
    {
        public CodedUITest2()
        {
        }

        [TestMethod]
        public void CodedUITestMethod1()
        {
            var appwindow = XamlWindow.Launch("{1EC6752D-401A-4B23-8801-D64ACF5D8D54}:App:Mobilexpense.MobileXpense_k2rrkyr26020p!App");

            UIMap.UIMobileXpenseWindow.UIHubHub.UIUsernameEdit.Text = "staging.tmobusr1";
            UIMap.UIMobileXpenseWindow.UIHubHub.UIPasswordEdit.Text = "Iphon3";

            Gesture.Tap(UIMap.UIMobileXpenseWindow.UIHubHub.UILoginButton);

            UIMap.UIMobileXpenseWindow.UIItemsListList.WaitForControlExist(30000);


        }

        public UIMap UIMap
        {
            get
            {
                if ((this.map == null))
                {
                    this.map = new UIMap();
                }

                return this.map;
            }
        }

        private UIMap map;
    }
}
