namespace Platform.System.Data.Exchange.ClientNetwork.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x14ce500fe11L)]
    public interface ClientSessionTemplate : Template
    {
        ClientSessionComponent clientSession();
    }
}

