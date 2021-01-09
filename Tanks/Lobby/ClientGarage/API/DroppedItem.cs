namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class DroppedItem
    {
        public override string ToString() => 
            $"marketItemEntity: {this.marketItemEntity}, Amount: {this.Amount}";

        public Entity marketItemEntity { get; set; }

        public int Amount { get; set; }
    }
}

