// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.Test.FaultInjection.Constants;
using Microsoft.Win32;

namespace Microsoft.Test.FaultInjection
{
    /// <summary>
    /// Provides facilities for registration and query of the fault injection COM engine.
    /// </summary>
    public static class ComRegistrar
    {
        #region Private Data

        private static bool registered = false;

        #endregion

        #region Public Members

        /// <summary>
        /// CLSID of the fault injection COM engine.
        /// </summary>
        public const string Clsid = EngineInfo.Engine_CLSID;

        /// <summary>
        /// Registers the fault injection COM engine.
        /// </summary>
        /// <param name="enginePathName">Path name of engine file.</param>
        public static void Register(string enginePathName)
        {
            if (!IsRegistered())
            {
                if (String.IsNullOrEmpty(enginePathName))
                {
                    enginePathName = CalculateEnginePath();
                }
                Process regsvr32 = Process.Start("regsvr32", "/s \"" + enginePathName + "\"");
                regsvr32.WaitForExit();
                if (regsvr32.ExitCode != 0)
                {
                    enginePathName = Path.GetFullPath(enginePathName);
                    string errorMessage = null;
                    switch (regsvr32.ExitCode)
                    {
                        case 3:
                            errorMessage = ApiErrorMessages.RegisterEngineFileNotFound;
                            break;
                        case 5:
                            errorMessage = ApiErrorMessages.RegisterEngineAccessDenied;
                            break;
                        default:
                            errorMessage = ApiErrorMessages.RegisterEngineFailed;
                            break;
                    }
                    throw new FaultInjectionException(
                        string.Format(CultureInfo.CurrentCulture, errorMessage, regsvr32.ExitCode, enginePathName));
                }
            }
        }

        /// <summary>
        /// Unregisters the fault injection COM engine.
        /// </summary>
        public static void Unregister(string enginePathName)
        {
            if (String.IsNullOrEmpty(enginePathName))
            {
                enginePathName = CalculateEnginePath();
            }
            Process regsvr32 = Process.Start("regsvr32", " /s /u \"" + enginePathName + "\"");
            regsvr32.WaitForExit();
            if (regsvr32.ExitCode != 0)
            {
                enginePathName = Path.GetFullPath(enginePathName);
                string errorMessage = null;
                switch (regsvr32.ExitCode)
                {
                    case 3:
                        errorMessage = ApiErrorMessages.RegisterEngineFileNotFound;
                        break;
                    case 5:
                        errorMessage = ApiErrorMessages.RegisterEngineAccessDenied;
                        break;
                    default:
                        errorMessage = ApiErrorMessages.RegisterEngineFailed;
                        break;
                }
                throw new FaultInjectionException(
                    string.Format(CultureInfo.CurrentCulture, errorMessage, regsvr32.ExitCode, enginePathName));
            }

        }

        /// <summary>
        /// Determines whether the fault injection engine has been registered in COM.
        /// </summary>
        /// <returns>true if the fault injection COM engine has been registered.</returns>
        public static bool IsRegistered()
        {
            return Registry.ClassesRoot.OpenSubKey(EngineInfo.Engine_RegistryKey) != null;
        }

        /// <summary>
        /// Supresses auto-registration behaviour of the fault injection API.
        /// </summary>
        public static void SuppressAutoRegister()
        {
            registered = true;
        }

        #endregion

        #region Internal Members

        internal static void AutoRegister()
        {
            if (registered) { return; }

            Register(null);
            registered = true;
        }

        #endregion

        #region Private Members

        private static string CalculateEnginePath()
        {
            string assemblyDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            //determine if we are in a 32 or 64 bit process
            if (Marshal.SizeOf(new IntPtr()) == 8)
            {
                assemblyDir = Path.Combine(assemblyDir, "FaultInjectionEngine\\x64\\");
            }
            else
            {
                assemblyDir = Path.Combine(assemblyDir, "FaultInjectionEngine\\x86\\");
            }
            return Path.Combine(assemblyDir, EngineInfo.FaultEngineFileName);
        }

        #endregion
    }
}