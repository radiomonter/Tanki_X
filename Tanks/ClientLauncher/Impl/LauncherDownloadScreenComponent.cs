namespace Tanks.ClientLauncher.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Tanks.Lobby.ClientNavigation.API;
    using UnityEngine;
    using UnityEngine.UI;

    public class LauncherDownloadScreenComponent : MonoBehaviour, Component, NoScaleScreen
    {
        public Text loadingInfo;
    }
}

