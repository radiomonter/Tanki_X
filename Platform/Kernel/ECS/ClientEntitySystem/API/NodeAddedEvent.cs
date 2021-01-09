namespace Platform.Kernel.ECS.ClientEntitySystem.API
{
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x3c469043bfab940dL)]
    public class NodeAddedEvent : Event
    {
        public static readonly NodeAddedEvent Instance = new NodeAddedEvent();
    }
}

