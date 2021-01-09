namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Lobby.ClientGarage.Impl;

    [SerialVersionUID(0x159b06b7b25L)]
    public interface GarageModulesScreenTemplate : Template
    {
        GarageModulesScreenComponent garageModulesScreen();
        [AutoAdded, PersistentConfig("", false)]
        ModulesScreenTextComponent modulesScreenText();
    }
}

