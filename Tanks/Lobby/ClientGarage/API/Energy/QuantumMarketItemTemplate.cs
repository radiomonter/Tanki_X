namespace Tanks.Lobby.ClientGarage.API.Energy
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Lobby.ClientGarage.API;

    [SerialVersionUID(0x8d4faa3dd0a1122L)]
    public interface QuantumMarketItemTemplate : EnergyItemTemplate, MarketItemTemplate, ItemImagedTemplate, GarageItemImagedTemplate, GarageItemTemplate, Template
    {
    }
}

