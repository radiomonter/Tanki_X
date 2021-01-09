namespace Lobby.ClientPayment.API
{
    using Lobby.ClientPayment.Impl;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x152fe7fb512L)]
    public interface SectionTemplate : Template
    {
        [AutoAdded]
        SectionComponent section();
    }
}

