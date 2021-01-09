namespace Platform.Kernel.ECS.ClientEntitySystem.API
{
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x1edc46893e4219c7L)]
    public class NodeRemoveEvent : Event
    {
        public static readonly NodeRemoveEvent Instance = new NodeRemoveEvent();
    }
}

