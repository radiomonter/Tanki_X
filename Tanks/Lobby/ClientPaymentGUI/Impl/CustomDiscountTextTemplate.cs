namespace Tanks.Lobby.ClientPaymentGUI.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x8d4f6b64158a856L)]
    public interface CustomDiscountTextTemplate : Template
    {
        CustomDiscountTextComponent customDiscountText();
    }
}

