namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class SelectedItemComponent : Component
    {
        public SelectedItemComponent()
        {
        }

        public SelectedItemComponent(Entity selectedItem)
        {
            this.SelectedItem = selectedItem;
        }

        public Entity SelectedItem { get; set; }
    }
}

