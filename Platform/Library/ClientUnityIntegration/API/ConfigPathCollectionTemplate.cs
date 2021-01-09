namespace Platform.Library.ClientUnityIntegration.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x8d3b08510444e02L)]
    public interface ConfigPathCollectionTemplate : Template
    {
        [AutoAdded, PersistentConfig("", false)]
        ConfigPathCollectionComponent configPathCollection();
    }
}

