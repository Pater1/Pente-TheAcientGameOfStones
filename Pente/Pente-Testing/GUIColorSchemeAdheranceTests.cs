using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Windows.Media;
using System.Windows;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

namespace Pente_Testing {
    /// <summary>
    /// Summary description for GUIColorSchemeAdheranceTests
    /// </summary>
    [TestClass]
    public class GUIColorSchemeAdheranceTests {
        string logRaw = "";
        [TestMethod]
        public void ReflectAndCheckColors() {
            logRaw = "";

            IEnumerable<Type> types = AppDomain
                .CurrentDomain
                .GetAssemblies()
                .SelectMany(a => a.GetTypes())
                ;//.Where(t => /*t.IsClass && t.FullName.Contains("Pente") &&*/ !(t is null));
            
            bool fail = false;
            foreach (Type t in types) {
                if (t.IsSubclassOf(typeof(UIElement)) && !t.IsInterface && !t.IsAnonymousType() && !t.IsAbstract && !t.IsArray) {
                    logRaw += $"\r\n {t.FullName}";
                    object o;
                    o = Activator.CreateInstance(t);

                    IEnumerable<Color> resourceColors = ExtractResourceColors(t, o);

                    object bg = t.GetProperty("Background")?.GetValue(o);
                    bool? bgGood = EvaluateColor(bg, resourceColors);
                    if (bgGood.HasValue && !bgGood.Value) {
                        fail = true;
                    }

                    object fg = t.GetProperty("Foreground")?.GetValue(o);
                    bool? fgGood = EvaluateColor(fg, resourceColors);
                    if (fgGood.HasValue && !fgGood.Value) {
                        fail = true;
                    }
                }
            }
            
            if (!Directory.Exists("../../Logs")) {
                Directory.CreateDirectory("../../Logs");
            }
            if (!File.Exists("../../Logs/ColorTests.log")) {
                File.Create("../../Logs/ColorTests.log");
            }
            File.WriteAllText("../../Logs/ColorTests.log", logRaw);

            //FileStream fs = File.OpenWrite("../../Logs/ColorTests.log");
            //fs.Write(logRaw.ToArray().Select(x => (byte)x).ToArray(), 0, logRaw.Length);

            if (fail) Assert.Fail();
        }

        private IEnumerable<Color> ExtractResourceColors(Type t, object o) {
            ResourceDictionary resources = t.GetProperty("Resources")?.GetValue(o) as ResourceDictionary;
            if (resources is null) return null;

            List<Color> ret = new List<Color>();
            foreach(object kv in resources) {
                if(kv is Color) {
                    ret.Add((Color)kv);
                }
            }
            return ret;
        }
        private bool? EvaluateColor(object toEval, IEnumerable<Color> acceptedSet) {
            if (toEval != null) {
                Color col = (Color)toEval;
                return acceptedSet.Contains(col);
            }
            return null;
        }
    }
    public static class TypeExtension {
        public static Boolean IsAnonymousType(this Type type) {
            Boolean hasCompilerGeneratedAttribute = type.GetCustomAttributes(typeof(CompilerGeneratedAttribute), false).Count() > 0;
            Boolean nameContainsAnonymousType = type.FullName.Contains("AnonymousType");
            Boolean isAnonymousType = hasCompilerGeneratedAttribute || nameContainsAnonymousType;

            return isAnonymousType;
        }
    }
}
