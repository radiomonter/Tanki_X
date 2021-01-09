namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;

    public interface GarageItemsRegistry
    {
        T GetItem<T>(Entity marketEntity) where T: GarageItem, new();
        T GetItem<T>(long marketId) where T: GarageItem, new();

        ICollection<TankPartItem> Hulls { get; }

        ICollection<TankPartItem> Turrets { get; }

        ICollection<ContainerBoxItem> Containers { get; }

        ICollection<VisualItem> Paints { get; }

        ICollection<VisualItem> Coatings { get; }

        ICollection<DetailItem> Details { get; }

        ICollection<ModuleItem> Modules { get; }
    }
}

