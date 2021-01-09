namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Lobby.ClientGarage.Impl;

    [SerialVersionUID(0x14e1e984ee2L)]
    public interface GarageTemplate : Template
    {
        [AutoAdded, PersistentConfig("", false)]
        LocalizedVisualPropertiesComponent localizedVisualProperties();
        [AutoAdded, PersistentConfig("", false)]
        ModuleTypesImagesComponent moduleTypesImages();
        [AutoAdded, PersistentConfig("", false)]
        SlotsTextsComponent slotsTexts();
        [AutoAdded, PersistentConfig("", false)]
        ItemUpgradeExperiencesConfigComponent upgradeLevels();
    }
}

