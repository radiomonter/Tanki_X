namespace Lobby.ClientPayment.API
{
    using Lobby.ClientPayment.Impl;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x1527d3ddaebL)]
    public interface PaymentMethodTemplate : Template
    {
        PaymentMethodComponent PaymentMethod();
    }
}

