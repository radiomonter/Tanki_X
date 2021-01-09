namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    [SerialVersionUID(0x8d52adf7474ba3fL)]
    public class TargetTierComponent : Component
    {
        private bool containsAllTierItem = true;
        private List<long> itemList = new List<long>();

        public int TargetTier { get; set; }

        public int MaxExistTier { get; set; }

        public bool ContainsAllTierItem
        {
            get => 
                this.containsAllTierItem;
            set => 
                this.containsAllTierItem = value;
        }

        public List<long> ItemList
        {
            get => 
                this.itemList;
            set => 
                this.itemList = value;
        }
    }
}

