namespace Tanks.Lobby.ClientPayment.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;

    public class RequestInfoForItemsEvent : Event
    {
        public readonly List<long> itemIds;
        public readonly Dictionary<long, string> titles = new Dictionary<long, string>();
        public readonly Dictionary<long, string> previews = new Dictionary<long, string>();
        public readonly Dictionary<long, bool> rarityFrames = new Dictionary<long, bool>();
        public readonly Dictionary<long, ItemRarityType> rarities = new Dictionary<long, ItemRarityType>();
        public readonly ICollection<long> purchased = new HashSet<long>();
        public string crystalTitle;
        public string crystalSprite;
        public string xCrystalTitle;
        public string xCrystalSprite;
        public string mainItemTitle;
        public string mainItemSprite;
        public string mainItemDescription;
        public int mainItemCount;
        public bool mainItemCrystal;
        public bool mainItemXCrystal;
        public long mainItemId;

        public RequestInfoForItemsEvent(List<long> itemIds)
        {
            this.itemIds = itemIds;
        }
    }
}

