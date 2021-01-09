namespace Tanks.Lobby.ClientNavigation.API
{
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientLogger.API;
    using Platform.System.Data.Statics.ClientConfigurator.API;
    using System;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using Tanks.Lobby.ClientNavigation.Impl;
    using UnityEngine;

    public class FatalErrorHandler
    {
        private static bool alreadyHandling;

        public static void HandleLog(string logString, string stackTrace, LogType type)
        {
            if (!IsErrorScreenWasShown && !alreadyHandling)
            {
                bool flag = (type == LogType.Exception) && !SkipShowScreen(logString, stackTrace);
                if ((((type == LogType.Error) || (type == LogType.Exception)) && !LogFromLog4Net(logString)) && !SkipSendToLog(logString))
                {
                    LoggerProvider.GetLogger<FatalErrorHandler>().ErrorFormat("{0}\n\n{1}", logString, stackTrace);
                }
                if ((flag & !Application.isEditor) & !Environment.GetCommandLineArgs().Contains<string>("-ignoreerrors"))
                {
                    alreadyHandling = true;
                    try
                    {
                        ShowFatalErrorScreen("clientlocal/ui/screen/error/unexpected");
                    }
                    finally
                    {
                        alreadyHandling = false;
                    }
                }
            }
        }

        private static ErrorScreenData LoadStringsFromConfig(string configPath) => 
            ConfiguratorService.GetConfig(configPath).ConvertTo<ErrorScreenData>();

        private static bool LogFromLog4Net(string logString) => 
            (logString != null) && logString.StartsWith("log4net:");

        private static void OverwriteNonEmptyFields(ErrorScreenData configFrom, ErrorScreenData configTo)
        {
            if (!string.IsNullOrEmpty(configFrom.HeaderText))
            {
                configTo.HeaderText = configFrom.HeaderText;
            }
            if (!string.IsNullOrEmpty(configFrom.ErrorText))
            {
                configTo.ErrorText = configFrom.ErrorText;
            }
            if (!string.IsNullOrEmpty(configFrom.RestartButtonLabel))
            {
                configTo.RestartButtonLabel = configFrom.RestartButtonLabel;
            }
            if (!string.IsNullOrEmpty(configFrom.ExitButtonLabel))
            {
                configTo.ExitButtonLabel = configFrom.ExitButtonLabel;
            }
            if (!string.IsNullOrEmpty(configFrom.ReportButtonLabel))
            {
                configTo.ReportButtonLabel = configFrom.ReportButtonLabel;
            }
            if (!string.IsNullOrEmpty(configFrom.ReportUrl))
            {
                configTo.ReportUrl = configFrom.ReportUrl;
            }
            configTo.ReConnectTime = configFrom.ReConnectTime;
        }

        public static void ShowBrokenConfigsErrorScreen()
        {
            if (!IsErrorScreenWasShown)
            {
                IsErrorScreenWasShown = true;
                ErrorScreenData data = new ErrorScreenData {
                    HeaderText = "ERROR",
                    ErrorText = "Required resources are corrupted or missing",
                    ReportButtonLabel = "REPORT",
                    ReportUrl = "https://help.tankix.com/en/tanki-x/articles/issues/initialization-issue",
                    ReConnectTime = 0xf423f,
                    ExitButtonLabel = "EXIT"
                };
                ErrorScreenData.data = data;
                SceneSwitcher.CleanAndSwitch(SceneNames.FATAL_ERROR);
            }
        }

        public static void ShowFatalErrorScreen(string configPath = "clientlocal/ui/screen/error/unexpected")
        {
            if (!IsErrorScreenWasShown)
            {
                IsErrorScreenWasShown = true;
                if (ConfiguratorService.HasConfig("clientlocal/ui/screen/error/common"))
                {
                    ErrorScreenData configTo = LoadStringsFromConfig("clientlocal/ui/screen/error/common");
                    if (ConfiguratorService.HasConfig(configPath))
                    {
                        OverwriteNonEmptyFields(LoadStringsFromConfig(configPath), configTo);
                    }
                    ErrorScreenData.data = configTo;
                }
                SceneSwitcher.CleanAndSwitch(SceneNames.FATAL_ERROR);
            }
        }

        private static bool SkipSendToLog(string logString) => 
            (logString != null) && (logString.Contains("The AssetBundle") || (logString.StartsWith("Failed opening GI file") || (logString.StartsWith("Failed loading Enlighten probe set data for hash") || logString.StartsWith("Error adding Enlighten probeset"))));

        private static bool SkipShowScreen(string logString, string stackTrace) => 
            (logString != null) && (logString.StartsWith("IndexOutOfRangeException") && stackTrace.Contains("TMPro.TMP_Text.FillCharacterVertexBuffers (Int32 i, Int32 index_X4)"));

        [Inject]
        public static ConfigurationService ConfiguratorService { get; set; }

        public static bool IsErrorScreenWasShown { get; set; }
    }
}

