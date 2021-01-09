namespace Tanks.Lobby.ClientLoading.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Platform.Library.ClientResources.API;

    [SerialVersionUID(0x6e1e2276dd4L)]
    public interface WarmupResourcesTemplate : Template
    {
        [AutoAdded, PersistentConfig("", false)]
        WarmupResourcesComponent warmupResources();
    }
}

