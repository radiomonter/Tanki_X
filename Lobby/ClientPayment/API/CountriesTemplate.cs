namespace Lobby.ClientPayment.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x8d380a0448b9abeL)]
    public interface CountriesTemplate : Template
    {
        [AutoAdded, PersistentConfig("", false)]
        CountriesComponent countries();
        [AutoAdded, PersistentConfig("", false)]
        PhoneCodesComponent phoneCodes();
    }
}

