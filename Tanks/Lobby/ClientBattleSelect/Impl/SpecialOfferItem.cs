namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using System;
    using System.Runtime.CompilerServices;

    public class SpecialOfferItem
    {
        public SpecialOfferItem()
        {
        }

        public SpecialOfferItem(int quantity, string spriteUid, string title) : this(quantity, spriteUid, title, string.Empty)
        {
        }

        public SpecialOfferItem(int quantity, string spriteUid, string title, string ribbonLabel)
        {
            this.Quantity = quantity;
            this.SpriteUid = spriteUid;
            this.Title = title;
            this.RibbonLabel = ribbonLabel;
        }

        public int Quantity { get; set; }

        public string SpriteUid { get; set; }

        public string Title { get; set; }

        public string RibbonLabel { get; set; }
    }
}

