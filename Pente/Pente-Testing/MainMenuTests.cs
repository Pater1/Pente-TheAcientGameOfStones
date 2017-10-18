using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
//using TestStack.White;
//using TestStack.White.UIItems.WindowItems;
//using TestStack.White.UIItems;
//using TestStack.White.UIItems.ListBoxItems;
//using TestStack.White.UIItems.Finders;
using FlaUI.Core;
using FlaUI.UIA3;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.AutomationElements.Infrastructure;
using System.Threading;

namespace Pente_Testing {
    [TestClass]
    public class MainMenuTests {
        [TestMethod]
        public void TestExit() {
            Application app = Application.Launch(TestResources.ApplicationPath);
            using (UIA3Automation automation = new UIA3Automation()) {
                Window window = app.GetMainWindow(automation);
                Button but = window.FindFirstDescendant(x => x.ByName("Exit")).AsButton();


                but.Click();

                bool fail = true;
                try {
                    window.Click();
                }catch{
                    fail = false;
                }

                Assert.IsFalse(fail);
            }
        }
        [TestMethod]
        public void TestGameStart_basic() {
            Application app = Application.Launch(TestResources.ApplicationPath);
            using (UIA3Automation automation = new UIA3Automation()) {
                Window window = app.GetMainWindow(automation);
                Button but = window.FindFirstDescendant(x => x.ByAutomationId("NewGameButton")).AsButton();

                but.Click();
                
                AutomationElement board = window.FindFirstDescendant(x => x.ByClassName("GameScreen"));

                Assert.IsNotNull(board);
            }
        }
    }
}
