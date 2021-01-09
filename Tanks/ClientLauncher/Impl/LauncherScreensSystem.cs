namespace Tanks.ClientLauncher.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientNavigation.API;

    public class LauncherScreensSystem : ECSSystem
    {
        [OnEventFire]
        public void ShowErrorScreen(ClientUpdateErrorEvent e)
        {
            FatalErrorHandler.ShowFatalErrorScreen("clientlocal/ui/screen/error/clientupdateerror");
        }

        [OnEventFire, Mandatory]
        public void StartDownload(StartDownloadEvent e, Node any, [JoinAll] DownloadScreenNode screenNode)
        {
            screenNode.textMapping.Text = screenNode.launcherDownloadScreenText.DownloadText;
        }

        [OnEventFire, Mandatory]
        public void StartReboot(StartRebootEvent e, Node any, [JoinAll] DownloadScreenNode screenNode)
        {
            screenNode.textMapping.Text = screenNode.launcherDownloadScreenText.RebootText;
        }

        public class DownloadScreenNode : Node
        {
            public LauncherDownloadScreenComponent launcherDownloadScreen;
            public LauncherDownloadScreenTextComponent launcherDownloadScreenText;
            public TextMappingComponent textMapping;
        }
    }
}

