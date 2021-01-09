namespace Tanks.ClientLauncher.Impl
{
    using Platform.Library.ClientResources.API;
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Security.Principal;
    using System.Threading;
    using Tanks.ClientLauncher.API;
    using UnityEngine;

    public static class ApplicationUtils
    {
        public static string GetAppRootPath() => 
            Directory.GetParent(Application.dataPath).FullName;

        public static string GetExecutablePathByName(string executable) => 
            GetExecutableRelativePathByName(executable);

        public static string GetExecutableRelativePathByName(string executable) => 
            executable.ToLower().EndsWith(".exe") ? executable : (executable + ".exe");

        public static void Restart()
        {
            string processName = Process.GetCurrentProcess().ProcessName;
            StartProcess(GetAppRootPath() + "/" + GetExecutablePathByName(processName), string.Empty);
            Application.Quit();
        }

        public static void StartProcess(string path, string args)
        {
            StartProcess(path, args, false);
        }

        private static void StartProcess(string path, string args, bool runAsAdministrator)
        {
            string subLine = new CommandLineParser(Environment.GetCommandLineArgs()).GetSubLine(LauncherConstants.PASS_THROUGH);
            string arguments = args + " " + subLine;
            path = WrapPath(path);
            Thread.Sleep(100);
            Console.WriteLine("Run process: " + path + " " + arguments);
            ProcessStartInfo startInfo = new ProcessStartInfo(path, arguments);
            string appRootPath = GetAppRootPath();
            string fullName = Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)).FullName;
            if (!appRootPath.Contains(fullName))
            {
                Console.WriteLine("try run as administrator");
            }
            else
            {
                Console.WriteLine("run as administrator is disabled, path {0} contains appData {1}", appRootPath, fullName);
                runAsAdministrator = false;
            }
            if (runAsAdministrator)
            {
                if (!new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator))
                {
                    startInfo.Verb = "runas";
                }
                else
                {
                    Console.WriteLine("run as administrator is disabled, user already have admin rules");
                }
            }
            Process.Start(startInfo);
        }

        public static void StartProcessAsAdmin(string path, string args)
        {
            StartProcess(path, args, true);
        }

        public static string WrapPath(string path) => 
            $""{path}"";
    }
}

