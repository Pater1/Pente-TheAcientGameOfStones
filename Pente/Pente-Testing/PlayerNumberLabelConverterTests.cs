using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pente.Converters;

namespace Pente_Testing {
    [TestClass]
    public class PlayerNameLabelConverterTests {
        [TestMethod]
        public void TestIntRange() {
            PlayerNameLabelConverter conv = new PlayerNameLabelConverter();
            int num = 2;

            Action<int> test = (Action<int>)((i) => {
                object pos = conv.Convert(i, typeof(string), null, null);
                object neg = conv.Convert(-i, typeof(string), null, null);

                Assert.IsTrue(pos is string);
                Assert.IsTrue(neg is string);

                string p_ative = pos as string;
                string n_ative = neg as string;
                
                Assert.IsTrue(p_ative.Contains("Stats"));
                Assert.IsTrue(p_ative.Contains(i.ToString()));
                
                Assert.IsTrue(n_ative.Contains("Stats"));
                Assert.IsTrue(n_ative.Contains((-i).ToString()));
            });

            while(num > 0) {
                test(num);

                num = num * 2;
            }


            test(int.MaxValue);
            test(-int.MaxValue);
            test(0);
        }
    }
}
