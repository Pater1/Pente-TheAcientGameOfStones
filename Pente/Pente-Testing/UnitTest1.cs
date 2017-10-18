using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pente.Models;

namespace Pente_Testing {
    [TestClass]
    public class StonesTests {
        [TestMethod]
        public void ProperInitializationTest_Stone() {
            Stone st = new Stone();
            Assert.Equals(st.CurrentState, StoneStates.Open);
        }
    }
}
