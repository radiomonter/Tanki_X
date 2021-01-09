namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientGarage.API;

    public class Avatar : VisualItem, IAvatarStateChanger, IComparable<Avatar>
    {
        private bool _unlocked = true;

        public int CompareTo(Avatar other) => 
            !ReferenceEquals(this, other) ? (!this.MarketItem.GetComponent<DefaultItemComponent>().Default ? (!other.MarketItem.GetComponent<DefaultItemComponent>().Default ? (((base.UserItem == null) || (other.UserItem != null)) ? (((other.UserItem == null) || (base.UserItem != null)) ? ((this.orderIndex == other.orderIndex) ? ((base.Rarity == other.Rarity) ? ((this.MinRank == other.MinRank) ? string.Compare(base.Name, other.Name, StringComparison.Ordinal) : (other.MinRank - this.MinRank)) : ((int) (other.Rarity - base.Rarity))) : (this.orderIndex - other.orderIndex)) : 1) : -1) : 1) : -1) : 0;

        public Action<bool> SetSelected { get; set; }

        public Action<bool> SetEquipped { get; set; }

        public Action<bool> SetUnlocked { get; set; }

        public Action OnBought { get; set; }

        public Action Remove { get; set; }

        public override Entity MarketItem
        {
            get => 
                base.MarketItem;
            set
            {
                base.MarketItem = value;
                this.IconUid = value.GetComponent<AvatarItemComponent>().Id;
                this.MinRank = value.GetComponent<PurchaseUserRankRestrictionComponent>().RestrictionValue;
                this.orderIndex = value.GetComponent<OrderItemComponent>().Index;
            }
        }

        public string RarityName =>
            base.Rarity.ToString().ToLower();

        public string IconUid { get; private set; }

        public int MinRank { get; private set; }

        public int Index { get; set; }

        private int orderIndex { get; set; }

        public bool Unlocked
        {
            get => 
                this._unlocked;
            set
            {
                this._unlocked = value;
                if (this.SetUnlocked != null)
                {
                    this.SetUnlocked(this._unlocked);
                }
            }
        }
    }
}

