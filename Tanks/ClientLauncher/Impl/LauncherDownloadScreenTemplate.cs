namespace Tanks.ClientLauncher.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x15060ad6d8aL)]
    public interface LauncherDownloadScreenTemplate : Template
    {
        [AutoAdded, PersistentConfig("", false)]
        LauncherDownloadScreenTextComponent launcherDownloadScreenText();
    }
}

