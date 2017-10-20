using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pente;
using System.Windows.Controls;

namespace Pente_Testing {
    [TestClass]
    public class MainWindowTests {
        [TestMethod]
        public void MainWindowCtorTest() {
            MainWindow mw = new MainWindow();
            Assert.IsTrue(mw.Logic != null);

            Grid mg = typeof(MainWindow).GetProperty("MainGrid").GetValue(mw) as Grid;
            //Assert.IsTrue(mg != null);
            Assert.IsTrue(mg.Children.Count > 0);
        }
    }
}
