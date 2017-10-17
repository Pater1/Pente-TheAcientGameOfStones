using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestStack.White;
using TestStack.White.Factory;
using TestStack.White.UIItems.WindowItems;

namespace Pente_Testing {
    public static class ClassLauncher {
        public static string applicationDirectory = @"..\Pente\bin\Debug";
        public static string applicationName = @"Pente.exe";
        public static string ApplicationPath {
            get {
                return Path.Combine(applicationDirectory, applicationName);
            }
        }

        public static void LaunchApp(out Window wind) {
            Application app = null;
            LaunchApp(out app, out wind);
        }
        public static void LaunchApp(out Application app, out Window wind) {
            LaunchApp(out app);
            wind = app.GetWindow("Pente", InitializeOption.NoCache);
        }
        public static void LaunchApp(out Application app) {
            app = Application.Launch(ApplicationPath);
        }
    }
}
