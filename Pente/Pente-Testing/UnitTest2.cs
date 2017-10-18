using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pente.UserControls;
using System.ComponentModel;

namespace Pente_Testing {
    [TestClass]
    public class PlayerStatsTests {
        [TestMethod]
        public void TestPropertyChangedEventHandler_Captures() {
            PlayerStatsUC psu = new PlayerStatsUC();

            bool callbackChecked = false;
            psu.PropertyChanged += ((e, b) => callbackChecked = true);

            psu.Captures = psu.Captures;
            Assert.IsTrue(callbackChecked);
        }
        [TestMethod]
        public void TestPropertyChangedEventHandler_PlayerNumber() {
            PlayerStatsUC psu = new PlayerStatsUC();

            bool callbackChecked = false;
            psu.PropertyChanged += ((e, b) => callbackChecked = true);

            psu.PlayerNumber = psu.PlayerNumber;
            Assert.IsTrue(callbackChecked);
        }
    }
}
