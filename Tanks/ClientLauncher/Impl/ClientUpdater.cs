namespace Tanks.ClientLauncher.Impl
{
    using Platform.Library.ClientDataStructures.API;
    using Platform.Library.ClientResources.API;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Threading;
    using Tanks.ClientLauncher.API;
    using UnityEngine;

    public static class ClientUpdater
    {
        private static readonly int PROCESS_STOP_TIMEOUT = 0x3a98;
        private static readonly int FILE_WAIT_TIMEOUT = 0x13880;
        private static readonly int FILE_WAIT_BEFORE_KILL_CONCURRENT_PROCESS = 0x2710;

        public static bool IsApplicationRunInUpdateMode()
        {
            string str;
            return new CommandLineParser(Environment.GetCommandLineArgs()).TryGetValue(LauncherConstants.UPDATE_PROCESS_COMMAND, out str);
        }

        public static bool IsUpdaterRunning()
        {
            string path = ApplicationUtils.GetAppRootPath() + "/update.lock";
            if (File.Exists(path))
            {
                try
                {
                    using (File.Open(path, FileMode.Open, FileAccess.Read))
                    {
                    }
                }
                catch (Exception)
                {
                    return true;
                }
            }
            return false;
        }

        private static void ReplaceProjectFiles(FileBackupTransaction transaction, string from, string to, string executableWithoutExtension)
        {
            List<Pair<string, string>> list = new List<Pair<string, string>>();
            List<Pair<string, string>> list2 = new List<Pair<string, string>>();
            foreach (string str in Directory.GetFiles(from, "*", SearchOption.AllDirectories))
            {
                string path = str.Replace(from, to);
                if (Path.GetFileNameWithoutExtension(path).Equals(executableWithoutExtension, StringComparison.OrdinalIgnoreCase))
                {
                    list2.Add(new Pair<string, string>(str, path));
                }
                else
                {
                    list.Add(new Pair<string, string>(str, path));
                }
            }
            foreach (Pair<string, string> pair in list)
            {
                Console.WriteLine("Copy project file from " + pair.Key + " to " + pair.Value);
                transaction.ReplaceFile(pair.Key, pair.Value);
            }
            foreach (Pair<string, string> pair2 in list2)
            {
                Console.WriteLine("Copy executable file from " + pair2.Key + " to " + pair2.Value);
                transaction.ReplaceFile(pair2.Key, pair2.Value);
            }
        }

        private static void TryKillOtherTankixProcesses()
        {
            try
            {
                Process currentProcess = Process.GetCurrentProcess();
                foreach (Process process2 in Process.GetProcessesByName(currentProcess.ProcessName))
                {
                    if (currentProcess.Id != process2.Id)
                    {
                        process2.Kill();
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("Kill process exception " + exception.Message);
            }
        }

        public static void Update()
        {
            UpdateReport report = new UpdateReport();
            FileBackupTransaction transaction = new FileBackupTransaction();
            string path = null;
            string to = null;
            string str3 = "unknown";
            string str4 = null;
            try
            {
                string appRootPath = ApplicationUtils.GetAppRootPath();
                CommandLineParser parser = new CommandLineParser(Environment.GetCommandLineArgs());
                int num = Convert.ToInt32(parser.GetValue(LauncherConstants.UPDATE_PROCESS_COMMAND));
                to = parser.GetValue(LauncherConstants.PARENT_PATH_COMMAND);
                str3 = parser.GetValue(LauncherConstants.VERSION_COMMAND);
                using (File.Open(to + "/update.lock", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
                {
                    string processName = Process.GetCurrentProcess().ProcessName;
                    WaitForProccessStop(Convert.ToInt32(num), PROCESS_STOP_TIMEOUT);
                    WaitForDropParentExecutable(transaction, to + "/" + ApplicationUtils.GetExecutableRelativePathByName(processName));
                    ReplaceProjectFiles(transaction, appRootPath, to, processName);
                    transaction.Commit();
                }
            }
            catch (Exception exception)
            {
                report.IsSuccess = false;
                report.Error = exception.Message;
                report.StackTrace = exception.StackTrace;
                transaction.Rollback();
            }
            finally
            {
                try
                {
                    report.UpdateVersion = str3;
                    WriteReport(to + "/" + LauncherConstants.REPORT_FILE_NAME, report);
                    if (!string.IsNullOrEmpty(str4) && File.Exists(str4))
                    {
                        File.Delete(str4);
                    }
                    ApplicationUtils.StartProcess(path, LauncherConstants.UPDATE_REPORT_COMMAND);
                }
                finally
                {
                    Application.Quit();
                }
            }
        }

        private static void WaitForDropParentExecutable(FileBackupTransaction transaction, string path)
        {
            if (File.Exists(path))
            {
                int num = 0;
                bool flag2 = false;
                while (true)
                {
                    bool flag = true;
                    try
                    {
                        Console.WriteLine("Try drop executable file " + path);
                        transaction.DeleteFile(path);
                        Console.WriteLine("Executable file droped");
                    }
                    catch (Exception)
                    {
                        flag = false;
                        if (num > FILE_WAIT_TIMEOUT)
                        {
                            throw;
                        }
                        if ((num > FILE_WAIT_BEFORE_KILL_CONCURRENT_PROCESS) && !flag2)
                        {
                            flag2 = true;
                            TryKillOtherTankixProcesses();
                        }
                        num += 0x3e8;
                        Thread.Sleep(0x3e8);
                    }
                    if (flag)
                    {
                        return;
                    }
                }
            }
        }

        private static bool WaitForProccessStop(int parentProcessId, int timeout)
        {
            Process processById;
            try
            {
                processById = Process.GetProcessById(parentProcessId);
            }
            catch (ArgumentException)
            {
                return true;
            }
            processById.WaitForExit(timeout);
            return processById.HasExited;
        }

        private static void WriteReport(string path, UpdateReport report)
        {
            try
            {
                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    report.Write(stream);
                }
            }
            catch (Exception exception1)
            {
                Debug.LogError(exception1);
            }
        }
    }
}

