using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using TestStack.White;
//using TestStack.White.Factory;
//using TestStack.White.UIItems.WindowItems;

namespace Pente_Testing {
    public static class TestResources {
        public static string ApplicationPath {
            get {
                return Directory.GetCurrentDirectory() + "\\Pente.exe";
            }
        }
    }
}
