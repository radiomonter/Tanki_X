namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientControls.API;

    public class VisibleItemsRangeComponent : Component
    {
        public VisibleItemsRangeComponent()
        {
        }

        public VisibleItemsRangeComponent(IndexRange range)
        {
            this.Range = range;
        }

        public IndexRange Range { get; set; }
    }
}

